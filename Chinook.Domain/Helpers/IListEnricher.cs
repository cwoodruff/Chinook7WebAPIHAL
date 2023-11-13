namespace Chinook.Domain.Helpers;

public interface IListEnricher
{
    Task<bool> Match(object target);
    Task Process(object representations);
}