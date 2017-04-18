using System.Collections.Generic;
using ModuleMainModule.Model;

namespace ModuleMainModule.Interfaces
{
    public interface IMovieService
    {
        void TakeMovie(MovieDTO movieDto);
        MovieDTO GetMovie(int? id);
        IEnumerable<MovieDTO> GetMovies();
        void Dispose();
    }
}