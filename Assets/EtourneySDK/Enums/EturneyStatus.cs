namespace Etourney.Enums
{
    public enum EtourneyStatus
    {
        Connecting,
        FailConnecting,
        Connected,
        Disconnecting,
        Disconnected,
        Disposing,
        Disposed,

        NotFoundKey,
        ServiceUnavailable,
        IncomingDataError
    }
}