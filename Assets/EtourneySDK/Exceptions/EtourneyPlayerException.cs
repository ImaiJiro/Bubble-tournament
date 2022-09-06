using System;

namespace Etourney.Exceptions
{
    public class EtourneyPlayerException : Exception
    {
        public EtourneyPlayerException(string message) : base(message)
        {
        }

        public EtourneyPlayerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}