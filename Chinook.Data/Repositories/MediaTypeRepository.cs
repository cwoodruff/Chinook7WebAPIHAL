using Chinook.Data.Data;
using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Data.Repositories;

public class MediaTypeRepository : IMediaTypeRepository
{
    private readonly ChinookContext _context;

    public MediaTypeRepository(ChinookContext context)
    {
        _context = context;
    }

    private bool MediaTypeExists(int id) =>
        _context.MediaTypes.Any(i => i.Id == id);

    public void Dispose() => _context.Dispose();

    public List<MediaType> GetAll() =>
        _context.MediaTypes.AsNoTrackingWithIdentityResolution().ToList();

    public MediaType GetById(int id) =>
        _context.MediaTypes.Find(id);

    public MediaType Add(MediaType newMediaType)
    {
        _context.MediaTypes.Add(newMediaType);
        _context.SaveChanges();
        return newMediaType;
    }

    public bool Update(MediaType mediaType)
    {
        if (!MediaTypeExists(mediaType.Id))
            return false;
        _context.MediaTypes.Update(mediaType);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        if (!MediaTypeExists(id))
            return false;
        var toRemove = _context.MediaTypes.Find(id);
        _context.MediaTypes.Remove(toRemove);
        _context.SaveChanges();
        return true;
    }
}