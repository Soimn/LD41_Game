using LudumDare41_Game.Content;
using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Entities;
using LudumDare41_Game.Physics;
using LudumDare41_Game.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.Towers {
    class TowerManager {

        private CoordHandler coordHandler;
        private ContentManager contentManager;
        private EntityManager entityManager;

        public List<Tower> Towers { get; }
        private List<Tower> previewTowers;

        public TowerManager (CoordHandler _coordHandler, ContentManager _contentManager, EntityManager _entityManager) {
            coordHandler = _coordHandler;
            contentManager = _contentManager;
            entityManager = _entityManager;
            Towers = new List<Tower>();
            previewTowers = new List<Tower>();
        }

        public void Update (GameTime gameTime) {
            if (Towers.Count != 0) {
                for (int i = 0; i < Towers.Count; i++)
                    Towers[i].Update(gameTime);
            }

            if (previewTowers.Count != 0) {
                for (int i = 0; i < previewTowers.Count; i++)
                    previewTowers[i].Update(gameTime);
            }
        }

        public void Draw (SpriteBatch spriteBatch) {
            if (Towers.Count != 0) {
                for (int i = 0; i < Towers.Count; i++)
                    Towers[i].Draw(spriteBatch);
            }

            if (previewTowers.Count != 0) {
                for (int i = 0; i < previewTowers.Count; i++)
                    previewTowers[i].Draw(spriteBatch);
            }
        }

        public void CreatePreviewTower (string towerID, TileCoord coord, out Tower retTower) {
            if (!Tower.towerID.TryGetValue(towerID, out Type type))
                throw new System.ArgumentException("Cannot possibly instantiate tower of type: {0}, because no such type exists", towerID);

            Object[] args = { this, contentManager, entityManager, true };

            Tower tower = (Tower)Activator.CreateInstance(type, args);

            Texture2D temp = contentManager.Load<Texture2D>("Towers/" + type.Name + "/" + type.Name + "_IdleSpritesheet");
            tower.Init(coord);
            previewTowers.Add(tower);
            retTower = tower;
        }

        public void DestroyPreviewTower (Tower tower) {
            previewTowers.Remove(tower);
        }

        public void SpawnTower (Tower tower, TileCoord coord) {
            tower.Init(coord);
            Towers.Add(tower);
            Console.WriteLine("Spawned tower at: {0}, {1}", tower.Coord.x, tower.Coord.y);

            Cards.cardsInHand.Remove(Cards.previouslyHeldCard); 
        }

        public Rectangle GetDrawRectangle (TileCoord coord, TowerWidth width, TowerHeight height) {
            return new Rectangle((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1, coordHandler.ScaleToZoom((int)width), coordHandler.ScaleToZoom((int)height));
        }

        public Vector2 GetDrawPos (TileCoord coord) {
            return new Vector2((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1);
        }

        public bool TowerAtCoord (TileCoord coord) {
            foreach (Tower tower in Towers) {
                if (tower.Coord == coord)
                    return true;
            }

            return false;
        }
    }
}
