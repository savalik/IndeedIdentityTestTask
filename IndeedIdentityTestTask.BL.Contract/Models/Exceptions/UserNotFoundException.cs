using System;

namespace IndeedIdentityTestTask.BL.Contract.Models.Exceptions
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException():base("User not found.")
        {
        }
    }
}
