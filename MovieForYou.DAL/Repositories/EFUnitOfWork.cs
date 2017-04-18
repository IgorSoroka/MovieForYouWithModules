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
            get
            {
                if (_movieRepository == null)
                    _movieRepository = new MovieRepository(_db);
                return _movieRepository;
            }
        }

        public IRepository<Show> Shows
        {
            get
            {
                if (_showRepository == null)
                    _showRepository = new ShowRepository(_db);
                return _showRepository;
            }
        }

        public IRepository<Actor> Actors
        {
            get
            {
                if (_actorRepository == null)
                    _actorRepository = new ActorRepository(_db);
                return _actorRepository;
            }
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