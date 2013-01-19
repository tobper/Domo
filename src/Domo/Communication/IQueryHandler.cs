using System.Threading.Tasks;

namespace Domo.Communication
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery
    {
        Task<TResult> Handle(TQuery query);
    }
}