using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chinook.Domain.Entities;

namespace Chinook.Domain.Repositories;

public interface IArtistRepository : IDisposable
{
    List<Artist> GetAll();
    Artist GetById(int id);
    Artist Add(Artist newArtist);
    bool Update(Artist artist);
    bool Delete(int id);
}