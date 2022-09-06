namespace Etourney.Scripts.WebSocket.AnswerProcedureHandlers.Interfaces
{
    internal interface IAnswerProcedureHandler
    {
        public void Handler(ushort context, ushort status, byte[] data);
    }
}