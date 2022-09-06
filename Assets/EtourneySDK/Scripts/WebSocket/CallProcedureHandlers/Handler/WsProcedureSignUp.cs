using System;
using System.Text;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignUp;
using Etourney.Scripts.WebSocket.CallProcedureHandlers.Interface;
using UnityEngine;
using WebSocket.Client;

namespace Etourney.Scripts.WebSocket.CallProcedureHandlers.Handler
{
    public class WsProcedureSignUp : ICallProcedureHandler
    {
        public void Handler(WebSocketClient webSocketClient, string guid, ushort context, object data)
        {
            try
            {
                var castData = (DtoWsProcedureInPlayerSignUp) data;
                var args = new WsCallProcedure<DtoWsProcedureInPlayerSignUp>(guid, context, castData);
                var bodySerialize = JsonUtility.ToJson(args);
                byte[] bodyBytes = Encoding.UTF8.GetBytes(bodySerialize);

                webSocketClient.Send(bodyBytes);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}