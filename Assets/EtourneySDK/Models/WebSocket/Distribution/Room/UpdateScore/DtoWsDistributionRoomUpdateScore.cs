using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Distribution.Room.UpdateScore
{
    [Serializable]
    public class DtoWsDistributionRoomUpdateScore
    {
        public long DeltaScore;

        public List<PlayerScore> Players;
        public Player PlayerWhoUpdated;

        [Serializable]
        public class PlayerScore : Player
        {
            public long Score;
        }

        [Serializable]
        public class Player
        {
            public string Key;
            public string Name;
        }
    }
}