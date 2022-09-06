using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Distribution.Room.Create
{
    [Serializable]
    public class DtoWsDistributionRoomCreateBracket
    {
        public long TournamentId;
        public string RoomGuid;

        public string RoomKey;
        public string BracketGuid;

        public List<Player> Players;

        [Serializable]
        public class Player
        {
            public string Key;
            public string Name;
            public string Avatar;
            public long Score;

            public int Round;
        }
    }
}