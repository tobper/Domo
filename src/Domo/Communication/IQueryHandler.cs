using System.Threading.Tasks;

namespace Domo.Communication
{
    public interface IQueryHandler<TResult, in TQuery>
        where TQuery : IQuery
    {
        Task<TResult> Handle(TQuery query);
    }
}