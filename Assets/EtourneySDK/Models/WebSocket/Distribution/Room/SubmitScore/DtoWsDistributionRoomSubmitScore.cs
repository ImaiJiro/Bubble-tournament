using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Distribution.Room.SubmitScore
{
    [Serializable]
    public class DtoWsDistributionRoomSubmitScore
    {
        public List<Player> Players { get; set; }

        [Serializable]
        public class Player
        {
            public string Name;

            public long Rating;

            public bool IsWin;
            public bool IsLoose;
            public bool IsDraw;
        }
    }
}