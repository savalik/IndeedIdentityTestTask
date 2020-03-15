using System.Threading.Tasks;

namespace IndeedIdentityTestTask.BL.Contract.External
{
    public interface IECBClient
    {
        Task<decimal> GetExchageRate(string currencyFrom, string currencyTo);
    }
}