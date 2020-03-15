using System;

namespace IndeedIdentityTestTask.BL.Contract.Models
{
    public class Exchange
    {
        public Currency CurrencyFrom { get; }
        public Currency CurrencyTo { get; }
        public decimal Amount { get; }

        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public Exchange(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            #region model validation
            if (amount <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must be positive.");
            }

            currencyFrom.CheckCurrency(nameof(currencyFrom));
            currencyTo.CheckCurrency(nameof(currencyTo));

            #endregion
            CurrencyFrom = currencyFrom;
            CurrencyTo = currencyTo;
            Amount = amount;
        }
    }
}
