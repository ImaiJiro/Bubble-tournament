using System;

namespace Etourney.Scripts.PublishSubscribe.Interfaces
{
    internal interface IListener
    {
        public void Publish(object data);

        public void Subscribe(Action<object> action);
        public void Unsubscribe(Action<object> action);
    }
}