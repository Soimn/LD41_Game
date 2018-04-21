using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

namespace LudumDare41_Game.World {
    class Level {
        public TiledMap map { get; private set; }
        TiledMapRenderer mapRenderer;
        string mapname;

        public Level(string mapToLoad, GraphicsDevice g) {
            mapname = mapToLoad;

            mapRenderer = new TiledMapRenderer(g);
        }

        public void Load(ContentManager c) {
            map = c.Load<TiledMap>("Levels/" + mapname);
        }

        public void Update(GameTime gt) {
            mapRenderer.Update(map, gt);
        }

        public void Draw(SpriteBatch sb, Camera2D camera, GraphicsDevice graphicsDevice) {
            var viewMatrix = camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0f, -1f);
            mapRenderer.Draw(map, viewMatrix, projectionMatrix);
        }
    }
}
