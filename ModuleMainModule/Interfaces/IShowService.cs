using System.Collections.Generic;
using ModuleMainModule.Model;

namespace ModuleMainModule.Interfaces
{
    public interface IShowService
    {
        void TakeShow(ShowDTO showDto);
        ShowDTO GetShow(int? id);
        IEnumerable<ShowDTO> GetShows();
        void DelShow(int id);
        void Dispose();
    }
}