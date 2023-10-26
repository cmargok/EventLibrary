using EventBusLibrary.Core.Events;

namespace EventBusLibrary.Core.Commands
{
    public abstract class Command : Message
    {
        public DateTime TimeSpam { get; protected set; }

        public Command()
        {
            TimeSpam = DateTime.Now;
        }
    }
}
