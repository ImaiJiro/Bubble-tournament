using System.Collections.Generic;
using System.Text;
using Etourney.Enums.WebSocket;
using Etourney.Models.WebSocket;
using Etourney.Scripts.WebSocket.AnswerProcedureHandlers.Handlers;
using Etourney.Scripts.WebSocket.AnswerProcedureHandlers.Interfaces;
using UnityEngine;

namespace Etourney.Scripts.WebSocket.AnswerProcedureHandlers
{
    internal class AnswerProcedureHandler
    {
        private readonly Dictionary<ushort, IAnswerProcedureHandler> _handlers;

        public AnswerProcedureHandler()
        {
            _handlers = new Dictionary<ushort, IAnswerProcedureHandler>
            {
                [(ushort) WebSocketProcedureContext.SignUpByEmail] = new WsReplySignUpByEmail(),
                [(ushort) WebSocketProcedureContext.SignInByEmail] = new WsReplySignInByEmail()
            };
        }

        public void Handler(byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                string body = Encoding.UTF8.GetString(data);
                var baseAnswer = JsonUtility.FromJson<WsAnswerProcedureBase>(body);

                if (_handlers.ContainsKey(baseAnswer.C))
                {
                    _handlers[baseAnswer.C].Handler(baseAnswer.C, baseAnswer.S, data);
                }
            }
        }
    }
}