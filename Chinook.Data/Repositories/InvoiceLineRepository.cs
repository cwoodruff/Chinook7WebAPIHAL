﻿using Chinook.Data.Data;
using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Data.Repositories;

public class InvoiceLineRepository : IInvoiceLineRepository
{
    private readonly ChinookContext _context;

    public InvoiceLineRepository(ChinookContext context)
    {
        _context = context;
    }

    private bool InvoiceLineExists(int id) =>
        _context.InvoiceLines.Any(i => i.Id == id);

    public void Dispose() => _context.Dispose();

    public List<InvoiceLine> GetAll() =>
        _context.InvoiceLines.AsNoTrackingWithIdentityResolution().ToList();

    public InvoiceLine GetById(int id) =>
        _context.InvoiceLines.Find(id);

    public InvoiceLine Add(InvoiceLine newInvoiceLine)
    {
        _context.InvoiceLines.Add(newInvoiceLine);
        _context.SaveChanges();
        return newInvoiceLine;
    }

    public bool Update(InvoiceLine invoiceLine)
    {
        if (!InvoiceLineExists(invoiceLine.Id))
            return false;
        _context.InvoiceLines.Update(invoiceLine);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        if (!InvoiceLineExists(id))
            return false;
        var toRemove = _context.InvoiceLines.Find(id);
        _context.InvoiceLines.Remove(toRemove);
        _context.SaveChanges();
        return true;
    }

    public List<InvoiceLine> GetByInvoiceId(int id) =>
        _context.InvoiceLines.Where(a => a.InvoiceId == id).AsNoTrackingWithIdentityResolution().ToList();

    public List<InvoiceLine> GetByTrackId(int id) =>
        _context.InvoiceLines.Where(a => a.TrackId == id).AsNoTrackingWithIdentityResolution().ToList();
}