using LudumDare41_Game.Content;
using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Entities;
using LudumDare41_Game.Physics;
using LudumDare41_Game.UI;
using LudumDare41_Game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LudumDare41_Game.Towers {
    class TowerManager {

        public CoordHandler coordHandler { get; }
        private ContentManager contentManager;
        private EntityManager entityManager;
        public Game1 Game { get; }

        public List<Tower> Towers { get; }
        private List<Tower> previewTowers;

        public TowerManager (CoordHandler _coordHandler, ContentManager _contentManager, EntityManager _entityManager, Game1 _game) {
            coordHandler = _coordHandler;
            contentManager = _contentManager;
            entityManager = _entityManager;
            Towers = new List<Tower>();
            previewTowers = new List<Tower>();
            Game = _game;
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

        public void SpawnTower (String towerID, TileCoord coord) {

            if (!Tower.towerID.TryGetValue(towerID, out Type type))
                throw new System.ArgumentException("Cannot possibly instantiate tower of type: {0}, because no such type exists", towerID);

            Object[] args = { this, contentManager, entityManager, false };

            Tower tower = (Tower)Activator.CreateInstance(type, args);

            Texture2D temp = contentManager.Load<Texture2D>("Towers/" + type.Name + "/" + type.Name + "_IdleSpritesheet");
            tower.Init(coord);
            Towers.Add(tower);

            Console.WriteLine("Spawned tower at: {0}, {1}", tower.Coord.x, tower.Coord.y);

        }

        public Rectangle GetDrawRectangle (TileCoord coord, TowerWidth width, TowerHeight height) {
            return new Rectangle((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1, coordHandler.ScaleToZoom((int)width), coordHandler.ScaleToZoom((int)height));
        }

        public Vector2 GetDrawPos (TileCoord coord) {
            return new Vector2((int)coordHandler.WorldToScreen(coord.ToVector2()).X + 1, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y + 1);
        }

        public bool InvalidCoord(TileCoord coord) {
            foreach (Tower tower in Towers) {
                if (tower.Coord == coord)
                    return true;
            }

            TileCoord homeCoord = new TileCoord((int)Home.position.X * 32, (int)Home.position.Y * 32);

            if (coord == homeCoord
                || coord == new TileCoord(homeCoord.x + 32, homeCoord.y)
                || coord == new TileCoord(homeCoord.x + 32, homeCoord.y - 32)
                || coord == new TileCoord(homeCoord.x, homeCoord.y - 32)
                || coord == new TileCoord(homeCoord.x, homeCoord.y + 32)
                || coord == new TileCoord(homeCoord.x + 32, homeCoord.y + 32)) {
                return true;
            }

            //Console.WriteLine(Game1.Level01.map.TileLayers[0].Tiles[coord.x].);
            var tileLayer = Game1.Level01.map.GetLayer<TiledMapTileLayer>("lay1");
            //Console.WriteLine((tileLayer.Tiles[(int)Math.Floor((double)coord.x / 32) + 32 * (int)Math.Floor((double)coord.y / 32)].GlobalIdentifier - 1) + "=" + Math.Floor((double)coord.x / 32) + ":" + Math.Floor((double)coord.y / 32));

            if (new[] { 5, 6, 7, 13, 14, 15, 21, 22, 23, 37, 38, 39, 45, 47, 40, 41, 42, 43, 48, 49, 50, 51, 53, 54}.Contains(tileLayer.Tiles[(int)Math.Floor((double)coord.x / 32) + 32 * (int)Math.Floor((double)coord.y / 32)].GlobalIdentifier - 1))
                return true;

            if (coord.y > 24 * 64)
                return true;

            return false;
        }
    }
}
