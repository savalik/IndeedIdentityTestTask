using IndeedIdentityTestTask.BL.Contract.External;
using IndeedIdentityTestTask.BL.Contract.Models;
using IndeedIdentityTestTask.BL.Contract.Models.Exceptions;
using IndeedIdentityTestTask.BL.Contract.Services;
using IndeedIdentityTestTask.DA.Contract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IndeedIdentityTestTask.BL.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletStorage _walletStorage;
        private readonly IECBClient _ecbClient;

        public WalletService(IWalletStorage walletStorage, IECBClient ecbClient)
        {
            _walletStorage = walletStorage;
            _ecbClient = ecbClient;
        }

        public async Task Exchange(Guid? userId, Exchange exchange)
        {
            try
            {
                Withdraw(userId ?? throw new UserNotFoundException(), new Money(exchange.Amount, exchange.CurrencyFrom));

                var exchangRate = await _ecbClient.GetExchageRate(
                    currencyFrom: exchange.CurrencyFrom.ToString(),
                    currencyTo: exchange.CurrencyTo.ToString());

                Replenish(userId, new Money(exchange.Amount * exchangRate, exchange.CurrencyTo));
                //Commit transaction
            }
            catch
            {
                //Abort transaction
                throw;
            }
        }

        public WalletStatus GetStatus(Guid? userId)
        {
            var walletStatus = _walletStorage.GetWalletStatus(
                userId ?? throw new UserNotFoundException());
            if(walletStatus == null)
                throw new UserNotFoundException();

            return walletStatus;
        }

        public void Replenish(Guid? userId, Money money)
        {
            var walletStatus = _walletStorage.GetWalletStatus(
                userId ?? throw new UserNotFoundException());
            if(walletStatus == null)
                throw new UserNotFoundException();

            var forReplenish = walletStatus.Moneys.FirstOrDefault(x => x.Currency == money.Currency);

            if (forReplenish == null)
                forReplenish = money;
            else
            {
                forReplenish = new Money(forReplenish.Amount + money.Amount, money.Currency);
            }

            Save(userId.Value, walletStatus, forReplenish);
        }

        public void Withdraw(Guid? userId, Money money)
        {
            var walletStatus = _walletStorage.GetWalletStatus(
                userId ?? throw new UserNotFoundException());
            if (walletStatus == null)
                throw new UserNotFoundException();

            var forWithdraw = walletStatus.Moneys.FirstOrDefault(x => x.Currency == money.Currency);

            if (forWithdraw == null)
                throw new NotEnoughMoneyException(money, null);
            else
            {
                if (money.Amount > forWithdraw.Amount)
                    throw new NotEnoughMoneyException(money, forWithdraw);

                forWithdraw = new Money(forWithdraw.Amount - money.Amount, money.Currency);
            }

            Save(userId.Value, walletStatus, forWithdraw);
        }

        private void Save(Guid userId, WalletStatus walletStatus, Money money)
        {
            _walletStorage.UpdateWallet(userId, new WalletStatus(
                            walletStatus.Moneys
                            .Where(x => x.Currency != money.Currency)
                            .Union(new[] { money })
                            .ToArray()));
        }
    }
}
