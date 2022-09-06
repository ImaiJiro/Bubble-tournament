using System.Collections.Generic;

namespace Etourney.Scripts.EtourneyTournaments
{
    public class RoomCredentials
    {
        public long TournamentId;
        public string RoomGuid;

        public string RoomKey;

        public List<Player> Players;
        
        public class Player
        {
            public string Key;
            public string Name;
            public string Avatar;
            public long Score;
        }
    }
}