using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class EmployeesEnricher : ListEnricher<List<EmployeeApiModel>>
{
    private readonly EmployeeEnricher _enricher;

    public EmployeesEnricher(EmployeeEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(List<object> representations)
    {
        foreach (var employee in representations)
        {
            await _enricher.Process(employee as EmployeeApiModel);
        }
    }
}