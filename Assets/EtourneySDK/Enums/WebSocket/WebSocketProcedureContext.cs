namespace Etourney.Enums.WebSocket
{
    internal enum WebSocketProcedureContext : ushort
    {
        SignUpByEmail = 1,
        SignInByEmail = 2,

        JoinTournament = 20,

        RoomPlayerUpdateScore = 40,
        RoomPlayerUpdateScoreBracket = 41,
        RoomPlayerSubmitScore = 42,
        RoomPlayerSubmitScoreBracket = 43,

        BracketGetHistory = 60,

        RandomInt = 70,
        RandomRangeInt = 71,
        RandomFloat = 72,

        PageTournaments = 100,
        PageGameCurrencyPrices = 101,

        DistributionBracketEndTournament = 65522,
        DistributionBracketNextRound = 65523,
        DistributionRoomPlayerLostConnection = 65524,
        DistributionRoomPlayerSubmitScore = 65525,
        DistributionRoomPlayerSubmitScoreBracket = 65526,
        DistributionRoomClose = 65527,
        DistributionRoomPlayerUpdateScoreBracket = 65528,
        DistributionRoomPlayerUpdateScore = 65529,

        DistributionRoomCreateSimple = 65530,
        DistributionRoomCreateMultiplayer = 65531,
        DistributionRoomCreateBracket = 65532,

        DistributionTournamentWaitQueueTimeoutBracket = 65533,
        DistributionTournamentWaitQueueTimeoutMultiplayer = 65534,
        DistributionTournamentWaitQueueTimeoutSimple = 65535
    }
}