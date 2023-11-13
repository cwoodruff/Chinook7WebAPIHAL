using Microsoft.AspNetCore.Mvc;

namespace Chinook.Domain.Helpers;

public abstract class ListEnricher<T> : IListEnricher
{
    public virtual Task<bool> Match(object target) =>
        target switch
        {
            List<T> => Task.FromResult(true),
            Task<List<T>> => Task.FromResult(true),
            OkObjectResult { Value: List<T> } => Task.FromResult(true),
            Task<OkObjectResult> { Result.Value: List<T> } => Task.FromResult(true),
            _ => Task.FromResult(false)
        };

    public abstract Task Process(object representations);
}