using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<InvoiceLineApiModel> GetAllInvoiceLine()
    {
        List<InvoiceLine> invoiceLines = _invoiceLineRepository.GetAll();
        var invoiceLineApiModels = invoiceLines.ConvertAll();

        foreach (var invoiceLine in invoiceLineApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("InvoiceLine-", invoiceLine.Id), invoiceLine, (TimeSpan)cacheEntryOptions);
        }

        return invoiceLineApiModels;
    }

    public InvoiceLineApiModel GetInvoiceLineById(int id)
    {
        var invoiceLineApiModelCached = _cache.Get<InvoiceLineApiModel>(string.Concat("InvoiceLine-", id));

        if (invoiceLineApiModelCached != null)
        {
            return invoiceLineApiModelCached;
        }
        else
        {
            var invoiceLine = _invoiceLineRepository.GetById(id);
            var invoiceLineApiModel = invoiceLine.Convert();
            invoiceLineApiModel.Track = GetTrackById(invoiceLineApiModel.TrackId);
            invoiceLineApiModel.Invoice = GetInvoiceById(invoiceLineApiModel.InvoiceId);
            if (invoiceLineApiModel.Track != null) invoiceLineApiModel.TrackName = invoiceLineApiModel.Track.Name;

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("InvoiceLine-", invoiceLineApiModel.Id), invoiceLineApiModel,
                (TimeSpan)cacheEntryOptions);

            return invoiceLineApiModel;
        }
    }

    public List<InvoiceLineApiModel> GetInvoiceLineByInvoiceId(int id)
    {
        var invoiceLines = _invoiceLineRepository.GetByInvoiceId(id);
        return invoiceLines.ConvertAll();
    }

    public List<InvoiceLineApiModel> GetInvoiceLineByTrackId(int id)
    {
        var invoiceLines = _invoiceLineRepository.GetByTrackId(id);
        return invoiceLines.ConvertAll();
    }

    public InvoiceLineApiModel AddInvoiceLine(InvoiceLineApiModel newInvoiceLineApiModel)
    {
        _invoiceLineValidator.ValidateAndThrowAsync(newInvoiceLineApiModel);

        var invoiceLine = newInvoiceLineApiModel.Convert();

        invoiceLine = _invoiceLineRepository.Add(invoiceLine);
        newInvoiceLineApiModel.Id = invoiceLine.Id;
        return newInvoiceLineApiModel;
    }

    public bool UpdateInvoiceLine(InvoiceLineApiModel invoiceLineApiModel)
    {
        _invoiceLineValidator.ValidateAndThrowAsync(invoiceLineApiModel);

        var invoiceLine = _invoiceLineRepository.GetById(invoiceLineApiModel.InvoiceId);
        
        invoiceLine.Id = invoiceLineApiModel.Id;
        invoiceLine.InvoiceId = invoiceLineApiModel.InvoiceId;
        invoiceLine.TrackId = invoiceLineApiModel.TrackId;
        invoiceLine.UnitPrice = invoiceLineApiModel.UnitPrice;
        invoiceLine.Quantity = invoiceLineApiModel.Quantity;

        return _invoiceLineRepository.Update(invoiceLine);
    }

    public bool DeleteInvoiceLine(int id)
        => _invoiceLineRepository.Delete(id);
}