using LudumDare41_Game.Content;
using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LudumDare41_Game.Towers {
    class TowerManager {

        private CoordHandler coordHandler;
        private ContentManager contentManager;

        private List<Tower> towers;
        private List<Tower> previewTowers;

        public TowerManager (CoordHandler _coordHandler, ContentManager _contentManager) {
            coordHandler = _coordHandler;
            contentManager = _contentManager;
            towers = new List<Tower>();
        }

        public void Update (GameTime gameTime) {
            if (towers.Count != 0) {
                for (int i = 0; i < towers.Count; i++)
                    towers[i].Update(gameTime);
            }
        }

        public void Draw (SpriteBatch spriteBatch) {
            if (towers.Count != 0) {
                for (int i = 0; i < towers.Count; i++)
                    towers[i].Draw(spriteBatch);
            }
        }

        public void DrawPreviewTower (SpriteBatch spriteBatch, Tower tower, TileCoord coord, ContentManager contentManager) {
            Texture2D temp = contentManager.Load<Texture2D>(nameof(tower) + "_IdleSpritesheet");
            //idleAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord));
        }

        public void SpawnTower (Tower tower, TileCoord coord) {
            tower.Init(coord);
            towers.Add(tower);
            System.Console.WriteLine("Spawned tower at: {0}, {1}", tower.Coord.x, tower.Coord.y);
        }

        public Rectangle GetDrawRectangle (TileCoord coord, TowerWidth width, TowerHeight height) {
            return new Rectangle((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1, coordHandler.ScaleToZoom((int)width), coordHandler.ScaleToZoom((int)height));
        }

        public Vector2 GetDrawPos (TileCoord coord) {
            return new Vector2((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1);
        }
    }
}
