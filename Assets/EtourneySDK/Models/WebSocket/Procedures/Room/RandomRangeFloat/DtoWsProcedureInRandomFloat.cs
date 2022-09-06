using System;

namespace Etourney.Models.WebSocket.Procedures.Room.RandomRangeFloat
{
    [Serializable]
    public class DtoWsProcedureInRandomFloat
    {
        public long TournamentId;
        public string RoomKey;
        public string PlayerToken;
    }
}