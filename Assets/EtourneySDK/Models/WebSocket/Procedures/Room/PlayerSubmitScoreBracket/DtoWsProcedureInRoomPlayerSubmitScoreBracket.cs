using System;

namespace Etourney.Models.WebSocket.Procedures.Room.PlayerSubmitScoreBracket
{
    [Serializable]
    public class DtoWsProcedureInRoomPlayerSubmitScoreBracket
    {
        public long TournamentId;
        public string RoomKey;
        public string BracketKey;

        public long Score;
    }
}