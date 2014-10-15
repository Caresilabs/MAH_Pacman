using MAH_Pacman.Entity.Components;
using MAH_Pacman.Entity.Systems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAH_Pacman.Entity
{
    public class GameEntity
    {
        private Dictionary<Type, Component> components;
        private Dictionary<Type, EntitySystem> systems;

        public GameEntity()
        {
            this.components = new Dictionary<Type, Component>();
            this.systems = new Dictionary<Type, EntitySystem>();
        }

        public void Update(float delta)
        {
            foreach (var system in systems)
            {
                system.Value.Update(delta);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var system in systems)
            {
                system.Value.Draw(batch);
            }
        }

        public void AddComponent(Component component)
        {
            try
            {
                components.Add(component.GetType(), component);
            }
            catch (System.ArgumentException e)
            {
                Console.WriteLine("Cannot add another " + component.GetType());
            } 
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component.GetType());
        }

        public void RemoveComponent<T>()
        {
            components.Remove(typeof(T));
        }

        public T GetComponent<T>()
        {
            return (T)Convert.ChangeType(components[typeof(T)], typeof(T));
        }

        public bool HasComponent<T>()
        {
            return components.ContainsKey(typeof(T));
        }

        public void AddSystem(EntitySystem system)
        {
            system.Entity = this;
            system.Init();
            systems.Add(system.GetType(), system);
        }

        public void RemoveSystem(EntitySystem system)
        {
            systems.Remove(system.GetType());
        }

        public void RemoveSystem<T>()
        {
            systems.Remove(typeof(T));
        }
    }
}
