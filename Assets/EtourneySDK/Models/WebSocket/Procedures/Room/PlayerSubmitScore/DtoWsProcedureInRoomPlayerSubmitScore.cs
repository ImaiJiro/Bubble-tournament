using System;

namespace Etourney.Models.WebSocket.Procedures.Room.PlayerSubmitScore
{
    [Serializable]
    public class DtoWsProcedureInRoomPlayerSubmitScore
    {
        public long TournamentId;
        public string RoomKey;

        public long Score;
    }
}