using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Distribution.Room.Create
{
    [Serializable]
    public class DtoWsDistributionRoomCreate
    {
        public long TournamentId;
        public string RoomGuid;

        public string RoomKey;

        public List<Player> Players;

        [Serializable]
        public class Player
        {
            public string Key;
            public string Name;
            public string Avatar;
            public long Score;
        }
    }
}