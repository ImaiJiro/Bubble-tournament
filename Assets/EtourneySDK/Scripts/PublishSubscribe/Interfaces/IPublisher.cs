using System;

namespace Etourney.Scripts.PublishSubscribe.Interfaces
{
    internal interface IPublisher
    {
        public event Action<object> Event;

        public void Publish(object data);
    }
}