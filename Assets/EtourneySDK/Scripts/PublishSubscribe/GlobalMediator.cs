using System;
using System.Collections.Generic;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Scripts.PublishSubscribe.Interfaces;

namespace Etourney.Scripts.PublishSubscribe
{
    internal class GlobalMediator
    {
        private static readonly object SyncRoot = new object();
        private static GlobalMediator _instance;

        private readonly Dictionary<EQueue, Dictionary<EChannel, IChannel>> _channelQueue;
        private readonly Dictionary<EQueue, Dictionary<EChannel, IListener>> _listenerQueue;

        private static GlobalMediator GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new GlobalMediator();
                    }
                }

                return _instance;
            }
        }

        private GlobalMediator()
        {
            _channelQueue = new Dictionary<EQueue, Dictionary<EChannel, IChannel>>();
            
            _listenerQueue = new Dictionary<EQueue, Dictionary<EChannel, IListener>>
            {
                { EQueue.WebSocket, new Dictionary<EChannel, IListener>() },
                { EQueue.Game, new Dictionary<EChannel, IListener>() },
                { EQueue.Player, new Dictionary<EChannel, IListener>() },
                { EQueue.Room, new Dictionary<EChannel, IListener>() }
            };
            
            _listenerQueue[EQueue.WebSocket].Add(EChannel.ChanelInWebSocket, new Listener());
            _listenerQueue[EQueue.WebSocket].Add(EChannel.ChanelOutWebSocket, new Listener());
            _listenerQueue[EQueue.WebSocket].Add(EChannel.Distribution, new Listener());

            _listenerQueue[EQueue.Game].Add(EChannel.ChanelGame, new Listener());
            _listenerQueue[EQueue.Player].Add(EChannel.ChanelPlayer, new Listener());
            _listenerQueue[EQueue.Room].Add(EChannel.ChanelRoom, new Listener());
        }

        public static void AddPublisher(EQueue queue, EChannel channel, IPublisher publisher)
        {
            lock (SyncRoot)
            {
                if (GetInstance._channelQueue.ContainsKey(queue))
                    if (GetInstance._channelQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._channelQueue[queue][channel].AddPublisher(publisher);
                    }
            }
        }

        public static void RemovePublisher(EQueue queue, EChannel channel, IPublisher publisher)
        {
            lock (SyncRoot)
            {
                if (GetInstance._channelQueue.ContainsKey(queue))
                    if (GetInstance._channelQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._channelQueue[queue][channel].RemovePublisher(publisher);
                    }
            }
        }

        public static void Subscribe(EQueue queue, EChannel channel, ISubscriber subscriber)
        {
            lock (SyncRoot)
            {
                if (GetInstance._channelQueue.ContainsKey(queue))
                    if (GetInstance._channelQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._channelQueue[queue][channel].Subscribe(subscriber);
                    }
            }
        }

        public static void Unsubscribe(EQueue queue, EChannel channel, ISubscriber subscriber)
        {
            lock (SyncRoot)
            {
                if (GetInstance._channelQueue.ContainsKey(queue))
                    if (GetInstance._channelQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._channelQueue[queue][channel].Unsubscribe(subscriber);
                    }
            }
        }

        public static void AddListener(EQueue queue, EChannel channel, Action<object> listener)
        {
            lock (SyncRoot)
            {
                if (GetInstance._listenerQueue.ContainsKey(queue))
                    if (GetInstance._listenerQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._listenerQueue[queue][channel].Subscribe(listener);
                    }
            }
        }

        public static void RemoveListener(EQueue queue, EChannel channel, Action<object> listener)
        {
            lock (SyncRoot)
            {
                if (GetInstance._listenerQueue.ContainsKey(queue))
                    if (GetInstance._listenerQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._listenerQueue[queue][channel].Unsubscribe(listener);
                    }
            }
        }

        public static void PublishInListeners(EQueue queue, EChannel channel, object data)
        {
            lock (SyncRoot)
            {
                if (GetInstance._listenerQueue.ContainsKey(queue))
                    if (GetInstance._listenerQueue[queue].ContainsKey(channel))
                    {
                        GetInstance._listenerQueue[queue][channel].Publish(data);
                    }
            }
        }
    }
}