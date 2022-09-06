using System;
using System.Runtime.InteropServices;
using Etourney.Enums;
using Etourney.Models.WebSocket;
using Etourney.Scripts.EtourneyGame;
using Etourney.Scripts.EtourneyPlayer;
using Etourney.Scripts.EtourneyTournaments;
using Etourney.Scripts.PublishSubscribe;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Scripts.WebSocket;
using Etourney.Settings;
using UnityEngine;

namespace Etourney.Scripts
{
    public class EtourneySDK
    {
        [DllImport("__Internal")]
        private static extern void openWindow(string url);

        private const string SdkVersion = "1.0.0";

        private string _apiLocalHost = "https://localhost:16732/api/v1/";
        private string _apiServerHost = "https://3.141.37.239:443/api/v1/";
        private int _apiTimeout = 5000;

        private readonly Uri _uriWebSocketLocal = new Uri("wss://localhost:16733/ws/v1");
        private readonly Uri _uriWebSocketServer = new Uri("wss://3.141.37.239:2083/ws/v1");

        private static readonly object SyncRoot = new object();
        private static EtourneySDK _instance;

        private readonly WebSocketWorker _webSocket;

        private Action<EtourneyStatus> _onStatus;
        private Action<int, int, float> _onConnectingToServer;
        private Action<ushort, ushort, string> _onDistribution;

        private readonly Game _game;
        private readonly Player _player;
        private readonly Tournaments _tournaments;

        private static EtourneySDK GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new EtourneySDK();
                    }
                }

                return _instance;
            }
        }

        private EtourneySDK()
        {
            //string httpServer = _apiLocalHost;
            //Uri webSocketServer = _uriWebSocketLocal;
            string httpServer = _apiServerHost;
            Uri webSocketServer = _uriWebSocketServer;

            if (SettingsLoader.Settings.ServerLocation == ServerLocation.Remote)
            {
                httpServer = _apiServerHost;
                webSocketServer = _uriWebSocketServer;
            }

            GlobalMediator.AddListener(EQueue.WebSocket, EChannel.Distribution, OnChanelDistribution);
            GlobalMediator.AddListener(EQueue.WebSocket, EChannel.ChanelOutWebSocket, OnChanelOutWebSocket);

            _webSocket = new WebSocketWorker(webSocketServer, OnWebSocketStatus, OnWebSocketConnectingToServer);

            _tournaments = new Tournaments();
            _game = new Game(httpServer, _apiTimeout);
            _player = new Player(httpServer, _apiTimeout);
        }

        public static void WebSocketConnect()
        {
            GetInstance._webSocket.Connect();
        }

        public static void WebSocketDisconnect()
        {
            GetInstance._webSocket.Disconnect();
        }

        private void OnChanelOutWebSocket(object data)
        {
            var strData = (string) data;

            Debug.Log(strData);
        }

        private void OnChanelDistribution(object data)
        {
            try
            {
                var strData = (string) data;

                Debug.Log(strData);

                var parseBase = JsonUtility.FromJson<WsAnswerProcedureBase>(strData);

                _onDistribution?.Invoke(parseBase.C, parseBase.S, strData);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private void OnWebSocketConnectingToServer(int countConnect, int totalCount, float timeOut)
        {
            _onConnectingToServer?.Invoke(countConnect, totalCount, timeOut);
        }

        private void OnWebSocketStatus(EtourneyStatus status)
        {
            _onStatus?.Invoke(status);
        }

        public static void OpenPaymentLink(string link)
        {
            if (!string.IsNullOrEmpty(link))
            {
                if (Uri.IsWellFormedUriString(link, UriKind.Absolute))
                {
                    if (Application.platform.Equals(RuntimePlatform.WebGLPlayer))
                    {
                        openWindow(link);
                    }
                    else
                    {
                        Application.OpenURL(link);
                    }
                }
            }
        }

        public static string SDKVersion
        {
            get { return SdkVersion; }
        }

        public static event Action<EtourneyStatus> OnStatus
        {
            add => GetInstance._onStatus += value;
            remove => GetInstance._onStatus -= value;
        }

        public static event Action<int, int, float> OnConnection
        {
            add => GetInstance._onConnectingToServer += value;
            remove => GetInstance._onConnectingToServer -= value;
        }

        public static event Action<ushort, ushort, object> OnDistribution
        {
            add => GetInstance._onDistribution += value;
            remove => GetInstance._onDistribution -= value;
        }

        public static Sprite CreateSpriteFromBase64(string dataBase64)
        {
            byte[] gameIconBytes = Convert.FromBase64String(dataBase64);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(gameIconBytes);

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100.0f);

            return sprite;
        }

        public static Tournaments Tournaments => GetInstance._tournaments;
        public static Game Game => GetInstance._game;
        public static Player Player => GetInstance._player;
    }
}