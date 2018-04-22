using LudumDare41_Game.Content;
using LudumDare41_Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    class EntityManager {

        private CoordHandler coordHandler;
        private ContentManager contentManager;

        public List<Entity> Entities { get; }
        public DummyEntity Dummy { get { return dummy; } }
        private DummyEntity dummy;

        public EntityManager(CoordHandler _coordHandler, ContentManager _contentManager) {
            coordHandler = _coordHandler;
            contentManager = _contentManager;

            EnemyEntity enemy = new EnemyEntity(_contentManager, _coordHandler, this);

            Entities = new List<Entity> {
                new DummyEntity(new Vector2(10000, 10000), out dummy)
            };

            SpawnEntity(enemy, new Vector2(320, 20));
        }

        public void SpawnEntity (Entity entity, Vector2 position) {
            entity.Init(position);
            Entities.Add(entity);

            System.Console.WriteLine("Spawned entity at {0}, {1}, {2}", position.X, position.Y, Entities.Contains(entity));
        }

        public void Update (GameTime gameTime) {
            for (int i = 0; i < Entities.Count; i++)
                Entities[i].Update(gameTime);
        }

        public void Draw (SpriteBatch spriteBatch) {
            for (int i = 0; i < Entities.Count; i++)
                Entities[i].Draw(spriteBatch);
        }

        public void Kill (Entity entity) {
            Entities.Remove(entity);
            //Effect
        }
    }
}
