using System;
using MovieForYou.DAL.Entities;

namespace MovieForYou.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Movie> Movies { get; }
        IRepository<Show> Shows  { get; }
        IRepository<Actor> Actors { get; }
        void Save();
    }
}