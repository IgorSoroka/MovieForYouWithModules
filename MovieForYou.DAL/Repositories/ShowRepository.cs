using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MovieForYou.DAL.EF;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Interfaces;

namespace MovieForYou.DAL.Repositories
{
    public class ShowRepository : IRepository<Show>
    {
        private readonly LocalDbContext _db;

        public ShowRepository(LocalDbContext context)
        {
            _db = context;
        }

        public IEnumerable<Show> GetAll()
        {
            return _db.Shows;
        }

        public Show Get(int id)
        {            
            return _db.Shows.Where(item => item.ExternalId == id).ToList().FirstOrDefault();
        }

        public void Create(Show show)
        {
            _db.Shows.Add(show);
        }

        public void Update(Show show)
        {
            _db.Entry(show).State = EntityState.Modified;
        }

        public IEnumerable<Show> Find(Func<Show, Boolean> predicate)
        {
            return _db.Shows.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Show show = _db.Shows.Where(item => item.ExternalId == id).ToList().FirstOrDefault();
            if (show != null)
                _db.Shows.Remove(show);
        }
    }
}