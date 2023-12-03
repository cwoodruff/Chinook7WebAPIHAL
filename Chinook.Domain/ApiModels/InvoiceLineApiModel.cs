﻿namespace Chinook.Domain.ApiModels;

public class InvoiceLineApiModel : BaseApiModel
{
    public int InvoiceId { get; set; }
    public int TrackId { get; set; }
    public string? TrackName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public InvoiceApiModel? Invoice { get; set; }

    public TrackApiModel? Track { get; set; }
}