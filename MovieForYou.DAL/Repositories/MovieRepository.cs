using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MovieForYou.DAL.EF;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Interfaces;

namespace MovieForYou.DAL.Repositories
{
    public class MovieRepository : IRepository<Movie>
    {
        private readonly LocalDbContext _db;

        public MovieRepository(LocalDbContext context)
        {
            _db = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _db.Movies;
        }

        public Movie Get(int id)
        {
            return _db.Movies.Find(id);
        }

        public void Create(Movie movie)
        {
            _db.Movies.Add(movie);
        }

        public void Update(Movie movie)
        {
            _db.Entry(movie).State = EntityState.Modified;
        }

        public IEnumerable<Movie> Find(Func<Movie, Boolean> predicate)
        {
            return _db.Movies.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Movie movie = _db.Movies.Find(id);
            if (movie != null)
                _db.Movies.Remove(movie);
        }
    }
}