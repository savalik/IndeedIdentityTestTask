using IndeedIdentityTestTask.BL.Contract.Models;
using IndeedIdentityTestTask.DA.Contract;
using System;
using System.Collections.Generic;

namespace IndeedIdentityTestTask.DA
{
    public class NaiveStorage : IWalletStorage
    {
        private readonly Dictionary<Guid, WalletStatus> _wallets = new Dictionary<Guid, WalletStatus>()
        {
            //This guid codes to "7tB8T_pVpE_m84qiEhls9g" base64 string
            { new Guid("4f7cd0ee-55ea-4fa4-a6f3-8aa212196cf6"), new WalletStatus(new[]
                {
                    new Money(100m, Currency.USD),
                    new Money(110000m, Currency.RUB),
                })},
        };

        public WalletStatus? GetWalletStatus(Guid userId)
        {
            _wallets.TryGetValue(userId, out var accountStatus);

            return accountStatus;
        }

        public void UpdateWallet(Guid userId, WalletStatus status)
        {
            _wallets[userId] = status;
        }
    }
}
