using System.Collections.Generic;
using AutoMapper;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Repositories;

namespace ModuleMainModule.Services
{
    public class ShowService : IShowService
    {
        readonly EFUnitOfWork _database = new EFUnitOfWork();

        public void TakeShow(ShowDTO showDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ShowDTO, Show>());
            Show show = Mapper.Map<ShowDTO, Show>(showDto);
            _database.Shows.Create(show);
            _database.Save();
        }

        public IEnumerable<ShowDTO> GetShows()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Show, ShowDTO>());
            return Mapper.Map<IEnumerable<Show>, List<ShowDTO>>(_database.Shows.GetAll());
        }

        public ShowDTO GetShow(int? id)
        {
            var show = _database.Shows.Get(id.Value);
            Mapper.Initialize(cfg => cfg.CreateMap<Show, ShowDTO>());
            return Mapper.Map<Show, ShowDTO>(show);
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}