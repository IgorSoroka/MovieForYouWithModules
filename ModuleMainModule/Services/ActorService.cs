using System.Collections.Generic;
using AutoMapper;
using ModuleMainModule.Interfaces;
using ModuleMainModule.Model;
using MovieForYou.DAL.Entities;
using MovieForYou.DAL.Repositories;

namespace ModuleMainModule.Services
{
    public class ActorService : IActorService
    {
        readonly EFUnitOfWork _database = new EFUnitOfWork();
        
        public void TakeActor(ActorDTO actorDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ActorDTO, Actor>());
            Actor actor = Mapper.Map<ActorDTO, Actor>(actorDto);
            _database.Actors.Create(actor);
            _database.Save();
        }

        public IEnumerable<ActorDTO> GetActors()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Actor, ActorDTO>());
            return Mapper.Map<IEnumerable<Actor>, List<ActorDTO>>(_database.Actors.GetAll());
        }

        public ActorDTO GetActor(int? id)
        {
            var actor = _database.Actors.Get(id.Value);
            Mapper.Initialize(cfg => cfg.CreateMap<Actor, ActorDTO>());
            return Mapper.Map<Actor, ActorDTO>(actor);
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}