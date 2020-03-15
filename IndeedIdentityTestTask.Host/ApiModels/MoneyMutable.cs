using System;
using IndeedIdentityTestTask.BL.Contract.Models;

namespace IndeedIdentityTestTask.Host.ApiModels
{
    public class MoneyMutable
    {
        public decimal Amount { get; set; }
        public string? Currency { get; set; }

        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public Money ToBl()
        {
            return new Money(
                amount: Amount,
                currency: Currency ?? string.Empty);
        }
    }
}
