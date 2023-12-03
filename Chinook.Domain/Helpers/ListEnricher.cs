namespace Chinook.Domain.Helpers;

public abstract class ListEnricher<T> : IEnricher
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