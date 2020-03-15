using System;
using System.Linq;

namespace IndeedIdentityTestTask.BL.Contract.Models
{
    //Naive realization, in real case Money can be struct (for example:
    //https://codereview.stackexchange.com/questions/165716/struct-for-money-and-currencies )
    [Serializable]
    public class Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public Money(decimal amount, string currency)
        {
            CheckAmount(amount);
            currency.CheckCurrency(nameof(currency));

            Amount = decimal.Round(amount, 4);
            Currency = DefineCurrency(currency);
        }

        public Money(decimal amount, Currency currency)
        {
            CheckAmount(amount);
            currency.CheckCurrency(nameof(currency));

            Amount = decimal.Round(amount, 4);
            Currency = currency;
        }

        private static Currency DefineCurrency(string currencyCode)
        {
            return Enum.TryParse<Currency>(currencyCode, true, out var currency) ? currency : 0;
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static void CheckAmount(decimal amount)
        {
            if (amount <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must be positive.");
            }
        }
    }
}
