using System;
using IndeedIdentityTestTask.BL.Contract.Models;

namespace IndeedIdentityTestTask.DA.Contract
{
    public interface IWalletStorage
    {
        WalletStatus? GetWalletStatus(Guid userId);
        void UpdateWallet(Guid userId, WalletStatus status);
    }
}
