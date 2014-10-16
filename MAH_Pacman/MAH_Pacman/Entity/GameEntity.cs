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
        
        public Engine Engine { get; set; }

        public GameEntity()
        {
            this.components = new Dictionary<Type, Component>();
        }

        public void Add(Component component)
        {
            try
            {
                components.Add(component.GetType(), component);
                if (Engine != null) Engine.UpdateEntitiesForSystems();
            }
            catch (System.ArgumentException)
            {
                Console.WriteLine("Cannot add another " + component.GetType());
            } 
        }

        public void Remove(Component component)
        {
            components.Remove(component.GetType());
            if (Engine != null) Engine.UpdateEntitiesForSystems();
        }

        public void Remove<T>()
        {
            components.Remove(typeof(T));
            if (Engine != null) Engine.UpdateEntitiesForSystems();
        }

        public T GetComponent<T>()
        {
            return (T)Convert.ChangeType(components[typeof(T)], typeof(T));
        }

        public List<Type> GetComponentTypes()
        {
            return components.Keys.ToList();
        }

        public bool HasComponent<T>()
        {
            return components.ContainsKey(typeof(T));
        }
    }
}
