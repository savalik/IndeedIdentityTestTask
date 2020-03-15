using System;
using IndeedIdentityTestTask.BL.Contract.Models;

namespace IndeedIdentityTestTask.Host.ApiModels
{
    public class ExchangeMutable
    {
        public string? CurrencyFrom { get; set; }
        public string? CurrencyTo { get; set; }
        public decimal Amount { get; set; }

        ///<exception cref="ArgumentOutOfRangeException"></exception>
        public Exchange ToBl()
        {
            return new Exchange(
                currencyFrom: Enum.TryParse(typeof(Currency), CurrencyFrom, out object? currencyFrom) && currencyFrom != null
                    ? (Currency)currencyFrom
                    : 0,
                currencyTo: Enum.TryParse(typeof(Currency), CurrencyTo, out object? currencyTo) && currencyTo != null
                    ? (Currency)currencyTo
                    : 0,
                amount: Amount);
        }
    }
}
