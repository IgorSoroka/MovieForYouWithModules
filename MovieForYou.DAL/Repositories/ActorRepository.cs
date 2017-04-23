using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MovieForYou.DAL.EF;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Interfaces;

namespace MovieForYou.DAL.Repositories
{
    public class ActorRepository : IRepository<Actor>
    {
        private readonly LocalDbContext _db;

        public ActorRepository(LocalDbContext context)
        {
            _db = context;
        }

        public IEnumerable<Actor> GetAll()
        {
            return _db.Actors;
        }

        public Actor Get(int id)
        {
            return _db.Actors.Where(item => item.ExternalId == id).ToList().FirstOrDefault();
        }

        public void Create(Actor actor)
        {
            _db.Actors.Add(actor);
        }

        public void Update(Actor actor)
        {
            _db.Entry(actor).State = EntityState.Modified;
        }

        public IEnumerable<Actor> Find(Func<Actor, Boolean> predicate)
        {
            return _db.Actors.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Actor actor = _db.Actors.Where(item => item.ExternalId == id).ToList().FirstOrDefault();
            if (actor != null)
                _db.Actors.Remove(actor);
        }
    }
}