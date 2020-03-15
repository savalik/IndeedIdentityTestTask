using System.Collections.Generic;
using System.Linq;

namespace IndeedIdentityTestTask.Host.ApiModels
{
    public class WalletStatus
    {
        public ICollection<MoneyMutable> Moneys { get; }

        public WalletStatus(BL.Contract.Models.WalletStatus walletStatus)
        {
            Moneys = walletStatus.Moneys.Select(x => 
                new MoneyMutable { 
                    Amount = x.Amount, 
                    Currency = x.Currency.ToString() 
                }).ToArray();
        }
    }
}
