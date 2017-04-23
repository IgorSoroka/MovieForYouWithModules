using System.Collections.Generic;
using ModuleMainModule.Model;

namespace ModuleMainModule.Interfaces
{
    public interface IActorService
    {
        void TakeActor(ActorDTO actorDto);
        ActorDTO GetActor(int? id);
        IEnumerable<ActorDTO> GetActors();
        void DelActor(int id);
        void Dispose();
    }
}