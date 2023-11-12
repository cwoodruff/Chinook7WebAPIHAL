using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<InvoiceApiModel> GetAllInvoice()
    {
        List<Invoice> invoices = _invoiceRepository.GetAll();
        var invoiceApiModels = invoices.ConvertAll();

        foreach (var invoice in invoiceApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Invoice-", invoice.Id), invoice, (TimeSpan)cacheEntryOptions);
        }

        return invoiceApiModels;
    }

    public InvoiceApiModel? GetInvoiceById(int id)
    {
        var invoiceApiModelCached = _cache.Get<InvoiceApiModel>(string.Concat("Invoice-", id));

        if (invoiceApiModelCached != null)
        {
            return invoiceApiModelCached;
        }
        else
        {
            var invoice = _invoiceRepository.GetById(id);
            var invoiceApiModel = invoice.Convert();
            invoiceApiModel.InvoiceLines = (GetInvoiceLineByInvoiceId(invoiceApiModel.Id)).ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Invoice-", invoiceApiModel.Id), invoiceApiModel, (TimeSpan)cacheEntryOptions);

            return invoiceApiModel;
        }
    }

    public List<InvoiceApiModel> GetInvoiceByCustomerId(int id)
    {
        var invoices = _invoiceRepository.GetByCustomerId(id);

        return invoices.ConvertAll();
    }

    public InvoiceApiModel AddInvoice(InvoiceApiModel newInvoiceApiModel)
    {
        _invoiceValidator.ValidateAndThrowAsync(newInvoiceApiModel);

        var invoice = newInvoiceApiModel.Convert();

        invoice = _invoiceRepository.Add(invoice);
        newInvoiceApiModel.Id = invoice.Id;
        return newInvoiceApiModel;
    }

    public bool UpdateInvoice(InvoiceApiModel invoiceApiModel)
    {
        _invoiceValidator.ValidateAndThrowAsync(invoiceApiModel);

        var invoice = _invoiceRepository.GetById(invoiceApiModel.Id);

        invoice.Id = invoiceApiModel.Id;
        invoice.CustomerId = invoiceApiModel.CustomerId;
        invoice.InvoiceDate = invoiceApiModel.InvoiceDate;
        invoice.BillingAddress = invoiceApiModel.BillingAddress ?? string.Empty;
        invoice.BillingCity = invoiceApiModel.BillingCity ?? string.Empty;
        invoice.BillingState = invoiceApiModel.BillingState ?? string.Empty;
        invoice.BillingCountry = invoiceApiModel.BillingCountry ?? string.Empty;
        invoice.BillingPostalCode = invoiceApiModel.BillingPostalCode ?? string.Empty;
        invoice.Total = invoiceApiModel.Total;

        return _invoiceRepository.Update(invoice);
    }

    public bool DeleteInvoice(int id)
        => _invoiceRepository.Delete(id);


    public List<InvoiceApiModel> GetInvoiceByEmployeeId(int id)
    {
        var invoices = _invoiceRepository.GetByEmployeeId(id);
        return invoices.ConvertAll();
    }
}