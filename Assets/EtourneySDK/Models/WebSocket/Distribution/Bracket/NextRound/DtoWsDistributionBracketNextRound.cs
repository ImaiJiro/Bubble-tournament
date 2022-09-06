using System;

namespace Etourney.Models.WebSocket.Distribution.Bracket.NextRound
{
    [Serializable]
    public class DtoWsDistributionBracketNextRound
    {
        public int Round;
        public string Key;

        public Enemy EnemyData;

        [Serializable]
        public class Enemy
        {
            public string Name;
            public string Avatar;
        }
    }
}