using System;

namespace Etourney.Models.WebSocket.Procedures.Room.RandomRangeInt
{
    [Serializable]
    public class DtoWsProcedureInRandomRangeInt
    {
        public long TournamentId;
        public string RoomKey;
        public string PlayerToken;

        public int Min;
        public int Max;
    }
}