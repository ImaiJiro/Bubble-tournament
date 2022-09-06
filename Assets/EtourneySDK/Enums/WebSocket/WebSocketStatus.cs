namespace Etourney.Enums.WebSocket
{
    public enum WebSocketStatus : ushort
    {
        Ok = 1,
        BadData = 2,
        Already = 3,
        NotAuthorized = 4,
        NotEnoughCurrency = 5,
        NotEnoughGameCoin = 6,
        RoomKeyError = 7,
        PlayerInvalidPassword = 8,

        BusyEmail = 40,

        EmptyPlayerToken = 80,
        EmptyGameKey = 81,
        EmptyEmail = 82,
        EmptyPassword = 83,
        EmptyName = 84,

        WrongPlayerToken = 120,
        WrongGameKey = 121,

        NotFoundPlayer = 140,
        NotFoundGame = 141,
        NotFoundTournament = 142,
        NotFoundRoom = 143,
        NotFoundBracketTournamentHistory = 144,

        LengthEmail = 180,
        LengthName = 181,

        ServerDistribution = 65534,
        InternalServerError = 65535
    }
}