using System.Threading.Tasks;

namespace Domo.Communication
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}