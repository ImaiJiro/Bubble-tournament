using System;
using System.Text;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Scripts.WebSocket.CallProcedureHandlers.Interface;
using UnityEngine;
using WebSocket.Client;

namespace Etourney.Scripts.WebSocket.CallProcedureHandlers.Handler
{
    public class WsProcedureSignIn : ICallProcedureHandler
    {
        public void Handler(WebSocketClient webSocketClient, string guid, ushort context, object data)
        {
            try
            {
                var castData = (DtoWsProcedureInPlayerSignIn) data;
                var args = new WsCallProcedure<DtoWsProcedureInPlayerSignIn>(guid, context, castData);
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