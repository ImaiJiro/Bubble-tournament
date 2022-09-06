using System;
using System.Collections.Generic;

namespace Etourney.Models.Api.Tournaments.Tournaments
{
    [Serializable]
    public class ResponseTournaments
    {
        public List<Tournament> Tournaments;

        public int Total;

        [Serializable]
        public class Tournament
        {
            public long Id;
            public string Name;
            public string Description;
            public string Icon;
            public int DurationMatch;
            public int TournamentWaitingTime;
            public int NumberTournaments;
            public int PlayersInTournament;

            public List<TournamentContributionCoinNode> TournamentContributionCoins;
            public List<TournamentContributionCurrencyNode> TournamentContributionCurrencies;
            public List<TournamentWinnerNode> TournamentWinners;
        }

        [Serializable]
        public class TournamentContributionCoinNode
        {
            public int Amount;
            public long GameCoinId;
        }

        [Serializable]
        public class TournamentContributionCurrencyNode
        {
            public string CurrencyCode;
            public string Amount;
        }

        [Serializable]
        public class TournamentWinnerNode
        {
            public short Place;

            public List<TournamentWinnersPayoutsCoinNode> TournamentWinnersPayoutsCoins;
            public List<TournamentWinnersPayoutsCurrencyNode> TournamentWinnersPayoutsCurrencies;
        }

        [Serializable]
        public class TournamentWinnersPayoutsCoinNode
        {
            public int Amount;
            public long GameCoinId;
        }

        [Serializable]
        public class TournamentWinnersPayoutsCurrencyNode
        {
            public string CurrencyCode;
            public string Amount;
        }
    }
}