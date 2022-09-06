using System;
using Etourney.Enums;
using Etourney.Scripts.PublishSubscribe;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Scripts.WebSocket.AnswerProcedureHandlers;
using Etourney.Scripts.WebSocket.CallProcedureHandlers;
using WebSocket.Client;
using WebSocket.Client.Common.Enums;

namespace Etourney.Scripts.WebSocket
{
    internal sealed class WebSocketWorker
    {
        private readonly WebSocketClient _webSocketClient;

        private readonly Action<EtourneyStatus> _onStatus;
        private readonly Action<int, int, float> _onConnectingToServer;

        private readonly CallProcedureHandler _callProcedureHandler;
        private readonly AnswerProcedureHandler _procedureHandler;

        public WebSocketWorker(Uri host, Action<EtourneyStatus> onStatus, Action<int, int, float> onConnectingToServer)
        {
            GlobalMediator.AddListener(EQueue.WebSocket, EChannel.ChanelInWebSocket, Listener);

            _onStatus = onStatus;
            _onConnectingToServer = onConnectingToServer;

            _procedureHandler = new AnswerProcedureHandler();

            _webSocketClient = new WebSocketClient(host, 5.0F, 128);

            _webSocketClient.OnConnectingToServer += ConnectingToServerHandler;
            _webSocketClient.OnStatusChange += StatusChangeHandler;
            _webSocketClient.OnIncomingData += IncomingDataHandler;

            _callProcedureHandler = new CallProcedureHandler(_webSocketClient);
        }

        public void Connect()
        {
            _webSocketClient.Connect();
        }

        public void Disconnect()
        {
            _webSocketClient.Disconnect();
        }

        private void Listener(object data)
        {
            var parseData = (CallProcedureData) data;

            _callProcedureHandler.Handler(parseData.Guid, parseData.Context, parseData.Data);
        }

        private void IncomingDataHandler(byte[] data)
        {
            _procedureHandler.Handler(data);
        }

        private void StatusChangeHandler(EClientStatus status)
        {
            switch (status)
            {
                case EClientStatus.Connecting:
                    _onStatus?.Invoke(EtourneyStatus.Connecting);
                    break;
                case EClientStatus.Connected:
                    _onStatus?.Invoke(EtourneyStatus.Connected);
                    break;
                case EClientStatus.Disconnecting:
                    _onStatus?.Invoke(EtourneyStatus.Disconnecting);
                    break;
                case EClientStatus.Disconnected:
                    _onStatus?.Invoke(EtourneyStatus.Disconnected);
                    break;
                case EClientStatus.Disposing:
                    _onStatus?.Invoke(EtourneyStatus.Disposing);
                    break;
                case EClientStatus.Disposed:
                    _onStatus?.Invoke(EtourneyStatus.Disposed);
                    break;
                case EClientStatus.FailConnecting:
                    _onStatus?.Invoke(EtourneyStatus.FailConnecting);
                    break;
            }
        }

        private void ConnectingToServerHandler(int countConnect, int totalCount, float timeOut)
        {
            _onConnectingToServer?.Invoke(countConnect, totalCount, timeOut);
        }
    }
}