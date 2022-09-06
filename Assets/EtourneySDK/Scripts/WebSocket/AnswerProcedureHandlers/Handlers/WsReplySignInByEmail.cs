using System;
using System.Text;
using Etourney.Enums.WebSocket;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Scripts.PublishSubscribe;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Scripts.WebSocket.AnswerProcedureHandlers.Interfaces;
using UnityEngine;

namespace Etourney.Scripts.WebSocket.AnswerProcedureHandlers.Handlers
{
    public class WsReplySignInByEmail : IAnswerProcedureHandler
    {
        public void Handler(ushort context, ushort status, byte[] data)
        {
            try
            {
                string body = Encoding.UTF8.GetString(data);
                var parseBody = JsonUtility.FromJson<WsAnswerProcedureBase>(body);

                GlobalMediator.PublishInListeners(EQueue.WebSocket, EChannel.ChanelOutWebSocket, body);

                if (parseBody.S == (ushort) WebSocketStatus.Ok)
                {
                    var parseData = JsonUtility.FromJson<WsAnswerProcedure<DtoWsProcedureOutPlayerSignIn>>(body);

                    GlobalWebSocketCallBack.Handler(parseBody.G, (WebSocketStatus) parseBody.S, parseData.Result);
                }
                else
                {
                    GlobalWebSocketCallBack.Handler(parseBody.G, (WebSocketStatus) parseBody.S, null);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}