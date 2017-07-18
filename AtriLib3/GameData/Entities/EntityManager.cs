using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.Entities
{
    public class EntityManager
    {
        private List<Entity> _entities;

        public void Add(Entity entity)
        {
            if (!entity.Initialized)
            {
                entity.LoadContent();
            }
            else
            {
                // TODO: Add add error stuff... MessageBox? or terminate app?...
            }

            _entities.Add(entity);
        }

        public void Remove(Entity e)
        {
            _entities.Remove(e);
        }

        public void RemoveAt(int index)
        {
            _entities.RemoveAt(index);
        }

        public List<Entity> GetEntities()
        {
            return _entities;
        }

        public Entity GetEntity(int index)
        {
            if(index > _entities.Count || index < 0)
            {
                return null;
            }

            return _entities[index];
        }

        public EntityManager()
        {
            _entities = new List<Entity>();
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < _entities.Count; i++)
            {
                _entities[i]?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i]?.Draw(spriteBatch);
            }
        }
    }
}
