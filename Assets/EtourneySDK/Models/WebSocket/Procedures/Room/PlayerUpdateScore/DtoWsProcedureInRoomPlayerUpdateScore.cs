using System;

namespace Etourney.Models.WebSocket.Procedures.Room.PlayerUpdateScore
{
    [Serializable]
    public class DtoWsProcedureInRoomPlayerUpdateScore
    {
        public long TournamentId;
        public string RoomKey;

        public long Score;
    }
}