using System;

namespace Etourney.Scripts.PublishSubscribe.Interfaces
{
    internal interface ISubscriber
    {
        public Action<object> Listener { get; }
    }
}