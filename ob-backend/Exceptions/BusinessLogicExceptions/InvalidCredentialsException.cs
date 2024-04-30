using System;

namespace ob.Exceptions.BusinessLogicExceptions
{
    [Serializable]
    public class InvalidCredentialException : Exception
    {
        public InvalidCredentialException(string message) : base(message)
        {
        }
    }
}