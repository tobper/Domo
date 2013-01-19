namespace Domo.Communication
{
    public interface IQueryHandler<in TQuery, out TResult>
        where TQuery : IQuery
    {
        TResult Handle(TQuery query);
    }
}