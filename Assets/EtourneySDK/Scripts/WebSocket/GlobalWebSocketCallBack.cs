using System;
using System.Collections.Generic;
using Etourney.Enums.WebSocket;

namespace Etourney.Scripts.WebSocket
{
    internal class GlobalWebSocketCallBack
    {
        private static readonly object SyncRoot = new object();
        private static GlobalWebSocketCallBack _instance;
        
        private readonly Dictionary<string, Action<WebSocketStatus, object>> _actions;

        private static GlobalWebSocketCallBack GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new GlobalWebSocketCallBack();
                    }
                }

                return _instance;
            }
        }

        private GlobalWebSocketCallBack()
        {
            _actions = new Dictionary<string, Action<WebSocketStatus, object>>();
        }

        public static string Add(Action<WebSocketStatus, object> action)
        {
            lock (SyncRoot)
            {
                var guid = GenerateRandomStringN();

                if (action == null)
                    return guid;

                if (!GetInstance._actions.ContainsKey(guid))
                {
                    GetInstance._actions.Add(guid, action);

                    return guid;
                }

                return string.Empty;
            }
        }

        public static void Handler(string guid, WebSocketStatus status, object data)
        {
            lock (SyncRoot)
            {
                if (GetInstance._actions.ContainsKey(guid))
                {
                    if (GetInstance._actions[guid] != null)
                    {
                        GetInstance._actions[guid].Invoke(status, data);
                    }

                    GetInstance._actions.Remove(guid);
                }
            }
        }

        private static string GenerateRandomStringN()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}