using System;

namespace Qek.Err
{
    public class CustomException : Exception
    {
        public CustomException()
        {

        }

        public CustomException(string message)
            : base(message)
        {

        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
