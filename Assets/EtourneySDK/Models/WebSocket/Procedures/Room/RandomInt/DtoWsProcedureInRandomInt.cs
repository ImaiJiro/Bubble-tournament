using System;

namespace Etourney.Models.WebSocket.Procedures.Room.RandomInt
{
    [Serializable]
    public class DtoWsProcedureInRandomInt
    {
        public long TournamentId;
        public string RoomKey;
        public string PlayerToken;
    }
}