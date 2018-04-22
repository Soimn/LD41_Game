using LudumDare41_Game.Content;
using LudumDare41_Game.Physics;
using LudumDare41_Game.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    class EntityManager {

        private CoordHandler coordHandler;
        private ContentManager contentManager;

        public List<Entity> Entities { get; }
        public DummyEntity Dummy { get { return dummy; } }
        private DummyEntity dummy;

        private Random r;
        Texture2D healthBar;

        public EntityManager (CoordHandler _coordHandler, ContentManager _contentManager) {
            coordHandler = _coordHandler;
            contentManager = _contentManager;

            healthBar = _contentManager.Load<Texture2D>("Entities/entityHealth");

            r = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);

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
            for (int i = 0; i < Entities.Count; i++) {
                Entities[i].Draw(spriteBatch);

                if (Entities[i] != dummy) { //Healthbar
                    Vector2 pos = new Vector2(coordHandler.WorldToScreen(Entities[i].Position).X , coordHandler.WorldToScreen(Entities[i].Position).Y - 45);
                    float length = (Entities[i].CurrentHealth / (float)Entities[i].Health) * 75;
                    if(length >= 75) {
                        spriteBatch.Draw(healthBar, new Rectangle((int)pos.X, (int)pos.Y, (int)length, 5), Color.LightGreen);
                    }
                    else if (length >= 37.5) {
                        spriteBatch.Draw(healthBar, new Rectangle((int)pos.X, (int)pos.Y, (int)length, 5), Color.Yellow);
                    }
                    else {
                        spriteBatch.Draw(healthBar, new Rectangle((int)pos.X, (int)pos.Y, (int)length, 5), Color.Red);
                    }
                }    
            }
        }

        public void Kill (Entity entity) {
            Entities.Remove(entity);
            entity = null;

            Cards.manaCurrent += r.Next(1, 3);
            //Effect
        }

        public bool IsAlive (Entity entity) {
            return Entities.Contains(entity);
        }

        public Entity GetLeadingEntity (List<Entity> entities) {

            if (entities.Count == 0)
                return null;

            int tempScore = 0;
            List<int> tempIndex = new List<int>();
            List<Entity> topScoreEntities = new List<Entity>();

            for (int i = 0; i < entities.Count; i++) {
                if (entities[i].Path[0].Score > tempScore) {
                    tempScore = entities[i].Path[0].Score;
                    tempIndex.Clear();
                    tempIndex.Add(i);
                }

                else if (entities[i].Path[0].Score == tempScore)
                    tempIndex.Add(i);
            }

            for (int i = 0; i < tempIndex.Count; i++) {
                topScoreEntities.Add(entities[i]);
            }

            float lenghtSquared = 10000; // arbitrarily large number
            int index = 0;
            for (int i = 0; i < topScoreEntities.Count; i++) {
                if ((topScoreEntities[i].Path[0].Position - topScoreEntities[i].Position).LengthSquared() < lenghtSquared) {
                    lenghtSquared = (topScoreEntities[i].Path[0].Position - topScoreEntities[i].Position).LengthSquared();
                    index = i;
                }
            }

            return topScoreEntities[index];
        }
    }
}
