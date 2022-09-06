using System;

namespace Etourney.Exceptions
{
    public class EtourneyGameKeyException : Exception
    {
        public EtourneyGameKeyException(string message) : base(message)
        {
        }

        public EtourneyGameKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}