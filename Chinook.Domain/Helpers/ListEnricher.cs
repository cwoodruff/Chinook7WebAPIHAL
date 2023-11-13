using Microsoft.AspNetCore.Mvc;

namespace Chinook.Domain.Helpers;

public abstract class ListEnricher<T> : IListEnricher
{
    public virtual Task<bool> Match(object target) =>
        target switch
        {
            T => Task.FromResult(true),
            Task<T> => Task.FromResult(true),
            _ => Task.FromResult(false)
        };

    public abstract Task Process(object representations);
}