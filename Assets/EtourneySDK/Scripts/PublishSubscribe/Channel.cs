using System.Collections.Generic;
using Etourney.Scripts.PublishSubscribe.Interfaces;

namespace Etourney.Scripts.PublishSubscribe
{
    internal class Channel : IChannel
    {
        private readonly object _sync = new object();
        private readonly List<ISubscriber> _subscribers;

        public Channel()
        {
            _subscribers = new List<ISubscriber>();
        }

        public void AddPublisher(IPublisher publisher)
        {
            lock (_sync)
            {
                publisher.Event += OnPublisher;
            }
        }

        public void RemovePublisher(IPublisher publisher)
        {
            lock (_sync)
            {
                publisher.Event -= OnPublisher;
            }
        }

        public void Subscribe(ISubscriber subscriber)
        {
            lock (_sync)
            {
                _subscribers.Add(subscriber);
            }
        }

        public void Unsubscribe(ISubscriber subscriber)
        {
            lock (_sync)
            {
                _subscribers.Remove(subscriber);
            }
        }

        private void OnPublisher(object data)
        {
            lock (_sync)
            {
                foreach (var subscriber in _subscribers)
                {
                    subscriber.Listener(data);
                }
            }
        }
    }
}