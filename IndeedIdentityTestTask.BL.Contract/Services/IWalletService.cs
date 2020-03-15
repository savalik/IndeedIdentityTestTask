using System;
using System.Threading.Tasks;
using IndeedIdentityTestTask.BL.Contract.Models;
using IndeedIdentityTestTask.BL.Contract.Models.Exceptions;

namespace IndeedIdentityTestTask.BL.Contract.Services
{
    public interface IWalletService
    {
        ///<exception cref="UserNotFoundException"></exception>
        WalletStatus GetStatus(Guid? userId);

        ///<exception cref="UserNotFoundException"></exception>
        void Replenish(Guid? userId, Money money);

        ///<exception cref="UserNotFoundException"></exception>
        ///<exception cref="NotEnoughMoneyException"></exception>
        void Withdraw(Guid? userId, Money money);

        ///<exception cref="UserNotFoundException"></exception>
        ///<exception cref="NotEnoughMoneyException"></exception>
        Task Exchange(Guid? userId, Exchange exchange);
    }
}
