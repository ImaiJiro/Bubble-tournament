using System;

namespace Etourney.Models.WebSocket.Distribution.Bracket.EndTournament
{
    [Serializable]
    public class DtoWsDistributionBracketEndTournament
    {
        public string RoomKey;
        public int Round;
    }
}