using System;

namespace Etourney.Models.WebSocket.Procedures.Room.PlayerUpdateScoreBracket
{
    [Serializable]
    public class DtoWsProcedureInPlayerUpdateScoreBracket
    {
        public long TournamentId;
        public string RoomKey;

        public long Score;
    }
}