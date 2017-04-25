using System;
using MovieForYou.DAL.EF;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Interfaces;

namespace MovieForYou.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly LocalDbContext _db;
        private MovieRepository _movieRepository;
        private ActorRepository _actorRepository;
        private ShowRepository _showRepository;
        private bool _disposed;

        public EFUnitOfWork()
        {
            _db = new LocalDbContext();
        }
        public IRepository<Movie> Movies
        {
            get { return _movieRepository ?? (_movieRepository = new MovieRepository(_db)); }
        }

        public IRepository<Show> Shows
        {
            get { return _showRepository ?? (_showRepository = new ShowRepository(_db)); }
        }

        public IRepository<Actor> Actors
        {
            get { return _actorRepository ?? (_actorRepository = new ActorRepository(_db)); }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}