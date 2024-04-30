using System;

namespace ob.Exceptions.BusinessLogicExceptions
{
    [Serializable]
    public class InvalidResourceException : Exception
    {
        public InvalidResourceException(string message) : base(message)
        {
        }
    }
}