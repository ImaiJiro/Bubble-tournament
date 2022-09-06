using WebSocket.Client;

namespace Etourney.Scripts.WebSocket.CallProcedureHandlers.Interface
{
    internal interface ICallProcedureHandler
    {
        public void Handler(WebSocketClient webSocketClient, string guid, ushort context, object data);
    }
}