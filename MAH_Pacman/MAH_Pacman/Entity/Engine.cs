using MAH_Pacman.Entity.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity
{
    public class Engine
    {
        private Dictionary<Type, EntitySystem> systems;
        private List<GameEntity> entities;

        public Engine()
        {
            this.systems = new Dictionary<Type, EntitySystem>();
            this.entities = new List<GameEntity>();
        }

        public void Update(float delta)
        {
            for (int i = 0; i < systems.Count; i++)
            {
                EntitySystem system = systems.Values.ToArray()[i];
                if (system.entities.Count != 0)
                    system.Update(delta);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var system in systems)
            {
                if (system.Value.GetType().IsSubclassOf(typeof(RenderSystem)))
                {
                    ((RenderSystem)system.Value).Draw(batch);
                }
            }
        }

        public void Add(GameEntity entity)
        {
            entity.engine = this;
            entities.Add(entity);
            UpdateEntitiesForSystems();
        }

        public void Remove(GameEntity entity)
        {
            entities.Remove(entity);
            UpdateEntitiesForSystems();
        }

        public void RemoveAllEntities()
        {
            entities.Clear();
            UpdateEntitiesForSystems();
        }

        public void Add(EntitySystem system)
        {
            system.engine = this;
            systems.Add(system.GetType(), system);
            UpdateEntitiesForSystems();
            system.Init();
        }

        public void UpdateEntitiesForSystems()
        {
            foreach (var system in systems)
            {
                UpdateEntitiesForSystem(system.Value);
            }
        }

        public void UpdateEntitiesForSystem(EntitySystem system)
        {
            List<GameEntity> list = system.entities;
            list.Clear();

            foreach (var entity in entities)
            {
                bool allowed = system.RequeredComponents().All(x => entity.GetComponentTypes().Contains(x));
                if (allowed)
                    list.Add(entity);
            }
        }

        public T GetSystem<T>()
        {

            return (T)Convert.ChangeType(systems[typeof(T)], typeof(T));
        }

        public void Remove(EntitySystem system)
        {
            systems.Remove(system.GetType());
        }

        public void Remove<T>()
        {
            systems.Remove(typeof(T));
        }
    }
}
