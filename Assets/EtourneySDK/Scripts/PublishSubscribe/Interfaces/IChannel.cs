namespace Etourney.Scripts.PublishSubscribe.Interfaces
{
    internal interface IChannel
    {
        public void AddPublisher(IPublisher publisher);
        public void RemovePublisher(IPublisher publisher);

        public void Subscribe(ISubscriber subscriber);
        public void Unsubscribe(ISubscriber subscriber);
    }
}