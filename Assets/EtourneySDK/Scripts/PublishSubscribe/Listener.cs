using System;
using System.Collections.Generic;
using Etourney.Scripts.PublishSubscribe.Interfaces;

namespace Etourney.Scripts.PublishSubscribe
{
    internal class Listener : IListener
    {
        private readonly object _sync = new object();
        private readonly List<Action<object>> _listeners;

        public Listener()
        {
            _listeners = new List<Action<object>>();
        }

        public void Subscribe(Action<object> action)
        {
            lock (_sync)
            {
                _listeners.Add(action);
            }
        }

        public void Unsubscribe(Action<object> action)
        {
            lock (_sync)
            {
                _listeners.Remove(action);
            }
        }

        public void Publish(object data)
        {
            lock (_sync)
            {
                foreach (var listener in _listeners)
                {
                    listener?.Invoke(data);
                }
            }
        }
    }
}