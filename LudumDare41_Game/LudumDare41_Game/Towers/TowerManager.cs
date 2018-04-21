using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LudumDare41_Game.Towers {
    class TowerManager {

        private CoordHandler coordHandler;

        private List<Tower> towers;

        public TowerManager (CoordHandler _coordHandler) {
            coordHandler = _coordHandler;
        }

        public void Update (GameTime gameTime) {
            for (int i = 0; i < towers.Count; i++)
                towers[i].Update(gameTime);
        }

        public void Draw (SpriteBatch spriteBatch) {
            for (int i = 0; i < towers.Count; i++)
                towers[i].Draw(spriteBatch);
        }

        public void SpawnTower (Tower tower, TileCoord coord) {
            tower.Init(coord);
        }

        public Rectangle GetDrawRectangle (TileCoord coord, TowerWidth width, TowerHeight height) {
            return new Rectangle((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1, coordHandler.ScaleToZoom((int)width), coordHandler.ScaleToZoom((int)height));
        }

        public Vector2 GetDrawPos (TileCoord coord) {
            return new Vector2((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1);
        }
    }
}
