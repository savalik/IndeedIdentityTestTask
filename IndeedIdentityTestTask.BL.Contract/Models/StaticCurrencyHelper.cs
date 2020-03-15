using System;
using System.Linq;

namespace IndeedIdentityTestTask.BL.Contract.Models
{
    public static class StaticCurrencyHelper
    {
        public static void CheckCurrency(this Currency currency, string fieldName)
        {
            if (!Enum.IsDefined(typeof(Currency), currency))
            {
                throw new ArgumentOutOfRangeException(
                    fieldName,
                    currency,
                    $"You can't use unavailable currency. Available currencies: " +
                    $"{ ListOfAvailableCurrencies}");
            }
        }

        public static void CheckCurrency(this string currency, string fieldName)
        {
            if (!Enum.IsDefined(typeof(Currency), currency))
            {
                throw new ArgumentOutOfRangeException(
                    fieldName,
                    currency,
                    $"You can't use unavailable currency. Available currencies: " +
                    $"{ ListOfAvailableCurrencies}");
            }
        }

        public static string ListOfAvailableCurrencies =>
            string.Join(",", ((Currency[])Enum
                    .GetValues(typeof(Currency)))
                    .Select(x => x.ToString())
                );
    }
}
