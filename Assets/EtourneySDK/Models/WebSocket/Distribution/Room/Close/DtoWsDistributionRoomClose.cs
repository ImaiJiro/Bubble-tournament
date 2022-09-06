using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Distribution.Room.Close
{
    [Serializable]
    public class DtoWsDistributionRoomClose
    {
        public List<Player> Players;

        [Serializable]
        public class Player
        {
            public string Key;
            public string Name;
            public byte Result;
            public long Score;
        }
    }
}