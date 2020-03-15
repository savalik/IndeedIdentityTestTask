using System;
using System.Threading.Tasks;
using IndeedIdentityTestTask.BL.Contract.Models;
using IndeedIdentityTestTask.BL.Contract.Models.Exceptions;
using IndeedIdentityTestTask.BL.Contract.Services;
using IndeedIdentityTestTask.Host.ApiModels;
using IndeedIdentityTestTask.Host.Converters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IndeedIdentityTestTask.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IWalletService _walletService;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService)
        {
            _logger = logger;
            _walletService = walletService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ApiModels.WalletStatus?> GetStatus(string id)
        {
            var userId = id.Base64ToGuid();

            try
            {
                var walletStatus = _walletService.GetStatus(userId);

                return new ApiModels.WalletStatus(walletStatus);
            }
            catch(UserNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User {userId} not found.");
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/replenish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Replenish(string id, MoneyMutable moneyMutable)
        {
            Money money;
            var userId = id.Base64ToGuid();
            try
            {
                money = moneyMutable.ToBl();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }

            try
            {
                _walletService.Replenish(userId, money);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User {userId} not found.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [HttpPost("{id}/withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult Withdraw(string id, MoneyMutable moneyMutable)
        {
            Money money;
            var userId = id.Base64ToGuid();
            try
            {
                money = moneyMutable.ToBl();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }

            try
            {
                _walletService.Withdraw(userId, money);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User {userId} not found.");
                return NotFound();
            }
            catch(NotEnoughMoneyException ex)
            {
                _logger.LogWarning(ex, $"User {userId} have not enough money.");
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [HttpPost("{id}/exchange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Exchange(string id, ExchangeMutable exchangeMutable)
        {
            var userId = id.Base64ToGuid();
            Exchange exchange;

            try
            {
                exchange = exchangeMutable.ToBl();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
            
            try
            {
                await _walletService.Exchange(userId, exchange);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, $"User {userId} not found.");
                return NotFound();
            }
            catch (NotEnoughMoneyException ex)
            {
                _logger.LogWarning(ex, $"User {userId} have not enough money.");
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
