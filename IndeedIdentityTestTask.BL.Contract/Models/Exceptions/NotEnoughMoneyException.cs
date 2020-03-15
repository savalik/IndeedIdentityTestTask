using System;

namespace IndeedIdentityTestTask.BL.Contract.Models.Exceptions
{
    [Serializable]
    public class NotEnoughMoneyException : Exception
    {
        public Money Requested { get; }
        public Money? Current { get; }
        public NotEnoughMoneyException(Money requested, Money? current) : base("User have not enough money.")
        {
            Requested = requested;
            Current = current;
        }
    }
}
