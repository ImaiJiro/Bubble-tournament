using System;

namespace Etourney.Exceptions
{
    public class EtourneyRoomException : Exception
    {
        public EtourneyRoomException(string message) : base(message)
        {
        }

        public EtourneyRoomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}