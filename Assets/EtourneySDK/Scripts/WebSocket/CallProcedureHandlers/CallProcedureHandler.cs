using System.Collections.Generic;
using Etourney.Enums.WebSocket;
using Etourney.Scripts.WebSocket.CallProcedureHandlers.Handler;
using Etourney.Scripts.WebSocket.CallProcedureHandlers.Interface;
using WebSocket.Client;

namespace Etourney.Scripts.WebSocket.CallProcedureHandlers
{
    internal class CallProcedureHandler
    {
        private readonly WebSocketClient _webSocketClient;
        private readonly Dictionary<ushort, ICallProcedureHandler> _handlers;

        public CallProcedureHandler(WebSocketClient webSocketClient)
        {
            _webSocketClient = webSocketClient;

            _handlers = new Dictionary<ushort, ICallProcedureHandler>
            {
                {(ushort) WebSocketProcedureContext.SignUpByEmail, new WsProcedureSignUp() },
                {(ushort) WebSocketProcedureContext.SignInByEmail, new WsProcedureSignIn() }
            };
        }

        public void Handler(string guid, ushort context, object data)
        {
            if (_handlers.ContainsKey(context))
            {
                _handlers[context].Handler(_webSocketClient, guid, context, data);
            }
        }
    }
}