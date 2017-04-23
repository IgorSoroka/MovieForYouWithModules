using System.Data.Entity;
using MovieForYou.DAL.Entities;

namespace MovieForYou.DAL.EF
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
            : base("LocalAppDb")
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Show> Shows { get; set; }
    }
}