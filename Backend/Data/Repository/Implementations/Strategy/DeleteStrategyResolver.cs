using Data.Repository.Interfaces.Strategy;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Enums;

namespace Data.Repository.Implementations.Strategy
{
    public class DeleteStrategyResolver<T> : IDeleteStrategyResolver<T> where T : class
    {
        private readonly IServiceProvider _provider;

        public DeleteStrategyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IDeleteStrategy<T> Resolve(DeleteType type)
        {
            return type switch
            {
                DeleteType.Logical => _provider.GetRequiredService<LogicalDeleteStrategy<T>>(),
                DeleteType.Permanent => _provider.GetRequiredService<PermanentDeleteStrategy<T>>(),
                _ => throw new NotImplementedException($"DeleteType {type} no está implementado.")
            };
        }
    }
}