using System.Collections.Generic;
using AutoMapper;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Repositories;

namespace ModuleMainModule.Services
{
    public class MovieService : IMovieService
    {
        private readonly EFUnitOfWork _database = new EFUnitOfWork();

        public void TakeMovie(MovieDTO movieDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<MovieDTO, Movie>());
            Movie movie = Mapper.Map<MovieDTO, Movie>(movieDto);
            _database.Movies.Create(movie);
            _database.Save();
        }

        public IEnumerable<MovieDTO> GetMovies()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Movie, MovieDTO>());
            return Mapper.Map<IEnumerable<Movie>, List<MovieDTO>>(_database.Movies.GetAll());
        }

        public MovieDTO GetMovie(int? id)
        {
            var movie = _database.Movies.Get(id.Value);
            Mapper.Initialize(cfg => cfg.CreateMap<Movie, MovieDTO>());
            return Mapper.Map<Movie, MovieDTO>(movie);
        }

        public void DelMovie(int id)
        {
            _database.Movies.Delete(id);
            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}