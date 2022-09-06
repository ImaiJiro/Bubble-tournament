using System;

namespace Etourney.Models.WebSocket.Procedures.Tournament.Tournaments.Tournaments
{
    [Serializable]
    public class DtoWsProcedureInTournaments
    {
        public int Start;
        public int Count;

        public bool OrderByDesc;
    }
}