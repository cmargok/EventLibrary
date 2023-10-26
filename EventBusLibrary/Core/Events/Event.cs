namespace EventBusLibrary.Core.Events
{
    public abstract class Event
    {
        public DateTime TimeSpam { get; protected set; }

        protected Event()
        {
            TimeSpam = DateTime.Now;
        }
    }
}
