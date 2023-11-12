﻿using Chinook.Data.Data;
using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Data.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly ChinookContext _context;

    public ArtistRepository(ChinookContext context)
    {
        _context = context;
    }

    private bool ArtistExists(int id) =>
        _context.Artists.Any(a => a.Id == id);

    public void Dispose() => _context.Dispose();

    public List<Artist> GetAll() =>
        _context.Artists.AsNoTrackingWithIdentityResolution().ToList();

    public Artist GetById(int id) =>
        _context.Artists.Find(id);

    public Artist Add(Artist newArtist)
    {
        _context.Artists.Add(newArtist);
        _context.SaveChanges();
        return newArtist;
    }

    public bool Update(Artist artist)
    {
        if (!ArtistExists(artist.Id))
            return false;
        _context.Artists.Update(artist);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        if (!ArtistExists(id))
            return false;
        var toRemove = _context.Artists.Find(id);
        _context.Artists.Remove(toRemove);
        _context.SaveChanges();
        return true;
    }
}