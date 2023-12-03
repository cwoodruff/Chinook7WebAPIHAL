namespace Chinook.Domain.ApiModels;

public class InvoiceApiModel : BaseApiModel
{
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string? BillingAddress { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingState { get; set; }
    public string? BillingCountry { get; set; }
    public string? BillingPostalCode { get; set; }
    public decimal Total { get; set; }

    public IList<InvoiceLineApiModel>? InvoiceLines { get; set; }

    public CustomerApiModel? Customer { get; set; }
}