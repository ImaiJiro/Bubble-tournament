namespace Etourney.Enums.Api
{
    internal enum ApiStatus : ushort
    {
        Ok = 1,
        BadData = 2,
        EmailBusy = 3,

        GameNotFound = 200,
        GameWrongKey = 201,

        GameCurrencyPriceNotFound = 220,

        TournamentsNotFound = 230,

        PlayerEmailIsEmpty = 500,
        PlayerEmailLengthError = 501,
        PlayerPasswordIsEmpty = 502,
        PlayerPasswordLengthError = 503,
        PlayerNotFound = 504,
        PlayerInvalidPassword = 505,

        InternalServerError = ushort.MaxValue
    }
}