using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Commands
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
