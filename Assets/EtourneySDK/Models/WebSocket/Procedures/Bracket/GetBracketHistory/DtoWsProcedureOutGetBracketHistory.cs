using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Procedures.Bracket.GetBracketHistory
{
    [Serializable]
    public class DtoWsProcedureOutGetBracketHistory
    {
        public long IntegerDateAdded;
        public string Guid;
        public long TournamentId;

        public BracketTournamentHistory History;

        [Serializable]
        public class BracketTournamentHistory
        {
            public int FinalRound;

            public List<WinnerPlace> Winners;
            public List<History> Histories;

            [Serializable]
            public class History
            {
                public long IntegerCreatedIn;
                public int Round;

                public List<string> ListOfParticipants;
                public List<Couple> Couples;
                public List<Winner> ListOfWinners;
                public List<string> ListOfLosers;
            }

            [Serializable]
            public class Couple
            {
                public string PlayerOne;
                public string PlayerTwo;
            }

            [Serializable]
            public class Winner
            {
                public byte Type;
                public string Player;
            }

            [Serializable]
            public class WinnerPlace
            {
                public string Player;
                public int Place;
            }

            public enum VictoryType : byte
            {
                ClearVictory,
                TechnicalVictory,
                Draw
            }
        }
    }
}