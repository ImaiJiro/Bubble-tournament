using System;

namespace Etourney.Models.Api.Tournaments.Tournaments
{
    [Serializable]
    public class RequestTournaments
    {
        public int Start;
        public int Count;

        public bool OrderByDesc;
    }
}