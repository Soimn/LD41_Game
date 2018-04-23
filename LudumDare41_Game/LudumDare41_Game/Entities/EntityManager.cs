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

        public CoordHandler CoordHandler { get; private set; }
        public ContentManager ContentManager { get; private set; }
        public WaveManager WaveManager { get; private set; }

        public List<Entity> Entities { get; }

        public Random Random { get; private set; }
        Texture2D healthBar;

        public EntityManager (CoordHandler _coordHandler, ContentManager _contentManager, WaveManager _waveManager) {
            CoordHandler = _coordHandler;
            ContentManager = _contentManager;
            WaveManager = _waveManager;

            healthBar = _contentManager.Load<Texture2D>("Entities/entityHealth");

            Random = new Random((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            Entities = new List<Entity>();
        }

        public void SpawnWaveEntity (Entity entity) {
            entity.Init_Wave(WaveManager.Path[0].Position + new Vector2(0, -96));
            Entities.Add(entity);
        }

        public void Update (GameTime gameTime) {
            for (int i = 0; i < Entities.Count; i++) { 
                Entities[i].Update(gameTime);

                if(Entities[i].Position.Y == 930)
            }
        }

        public void Draw (SpriteBatch spriteBatch) {
            for (int i = 0; i < Entities.Count; i++) {
                Entities[i].Draw(spriteBatch);


                Vector2 pos = new Vector2(CoordHandler.WorldToScreen(Entities[i].Position).X, CoordHandler.WorldToScreen(Entities[i].Position).Y - 45);
                float length = (Entities[i].CurrentHealth / (float)Entities[i].Health) * 75;
                if (length >= 75) {
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

        public void Kill (Entity entity) {
            Entities.Remove(entity);
            entity = null;

            Cards.manaCurrent += Random.Next(1, 3);
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

                if (entities[i].Path.Count == 0)
                    topScoreEntities.Add(entities[i]);

                else {
                    if (entities[i].Path[0].Score > tempScore) {
                        tempScore = entities[i].Path[0].Score;
                        tempIndex.Clear();
                        tempIndex.Add(i);
                    }

                    else if (entities[i].Path[0].Score == tempScore)
                        tempIndex.Add(i);
                }
            }

            for (int i = 0; i < tempIndex.Count; i++) {
                topScoreEntities.Add(entities[i]);
            }

            if (topScoreEntities[0].Path.Count == 0) {
                int tempWeakness = 10000; // arbitrarily large number
                int index_2 = 0;
                for (int j = 0; j < topScoreEntities.Count; j++) {
                    if ((int)topScoreEntities[j].Health < tempWeakness) {
                        tempWeakness = (int)topScoreEntities[j].Health;
                        index_2 = j;
                    }
                }

                return topScoreEntities[index_2];
            }

            else {
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

        public Type GetEntityOfTypeByGrade (int grade = 1) {

            switch (grade) {

                case 1:
                    return typeof(Entity_Bart);
                case 2:
                    return typeof(Entity_Bob);

                default:
                case 0:
                    throw new ArgumentException("Parameter grade in GetEntityOfTypeByGrade, cannot possiblt be " + grade);
            }
        }
    }
}
