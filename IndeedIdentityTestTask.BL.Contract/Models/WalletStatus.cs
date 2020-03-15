using System;
using System.Collections.Generic;

namespace IndeedIdentityTestTask.BL.Contract.Models
{
    [Serializable]
    public class WalletStatus
    {
        public WalletStatus(ICollection<Money> moneys)
        {
            if(moneys == null || moneys.Count <= 0)
            {
                throw new ArgumentException(
                    message: "A wallet must have at least one currency account.",
                    paramName: nameof(moneys));
            }

            Moneys = moneys;
        }

        public ICollection<Money> Moneys { get; }
    }
}
