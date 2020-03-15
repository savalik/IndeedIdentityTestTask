using IndeedIdentityTestTask.BL.Contract.External;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IndeedIdentityTestTask.BL.External
{
    public class ECBNaiveClient : IECBClient
    {
        private const string ECB_URL = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
        private readonly Dictionary<string, decimal> _ratesFromEUR = new Dictionary<string, decimal>()
        {
            { "EUR", 1m }
        };

        private DateTimeOffset _lastSyncOn;

        public async Task<decimal> GetExchageRate(string currencyFrom, string currencyTo)
        {
            if (DateTimeOffset.UtcNow > _lastSyncOn.AddMinutes(30))
            {
                await UpdateRates();
            }

            var exchangeRate = _ratesFromEUR[currencyTo] / _ratesFromEUR[currencyFrom];

            return exchangeRate;
        }

        private async Task UpdateRates()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(ECB_URL);
            var content = await response.Content.ReadAsStringAsync();

            //Cheap solution. But ok for parse xml which not changes since 2002. (but I'm not sure about that)
            using StringReader reader = new StringReader(content);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("currency"))
                {
                    var currency = line.Substring(19, 3);
                    var rate = line.Substring(30, line.Length - 33);

                    _ratesFromEUR[currency] = decimal.Parse(rate, System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            _lastSyncOn = DateTimeOffset.UtcNow;
        }
    }
}
