using System.Collections.Generic;

namespace Lucky38.TestReact.EntityScene
{
    public class RepositoryEntity  
    {
        private readonly Dictionary<int, Entity> _entities = new();

        public void Register(Entity entity) => _entities[entity.Id] = entity;
        public void Unregister(Entity entity) => _entities.Remove(entity.Id);
        public Entity Get(int id) => _entities.TryGetValue(id, out var e) ? e : null;
    }
}