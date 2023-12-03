using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class EmployeesEnricher(EmployeeEnricher enricher) : ListEnricher<List<EmployeeApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var employee in (IEnumerable<EmployeeApiModel>)representations)
        {
            await enricher.Process(employee as EmployeeApiModel);
        }
    }
}