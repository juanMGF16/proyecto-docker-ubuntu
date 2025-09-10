using Utilities.Enums;

namespace Data.Repository.Interfaces.Strategy
{
    public interface IDeleteStrategyResolver<T> where T : class
    {
        IDeleteStrategy<T> Resolve(DeleteType type);
    }
}