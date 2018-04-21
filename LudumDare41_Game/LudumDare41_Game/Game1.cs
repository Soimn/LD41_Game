using LudumDare41_Game.Content;
using LudumDare41_Game.Towers;
using LudumDare41_Game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace LudumDare41_Game.Physics {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { MENU, INGAME, PAUSE }; //gamestates, legg til om vi trenger
        GameStates currentState = GameStates.INGAME;

        private Camera2D camera;

        Level level01;
        Rectangle selectedTile; //Disse to er bare for debug om screen til world coordinater
        Texture2D selectTex;

        #region // Towers //

        private TowerManager towerManager;
        private ContentManager contentManager;
        private CoordHandler coordHandler;

        private float towerSpawnCooldown = 0.5f;
        private float lastTowerSpawn = 0;

        #endregion


        public Game1 () {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize () {
            camera = new Camera2D(GraphicsDevice) {
                Zoom = 2f,
                Origin = Vector2.Zero
            };

            selectedTile.Width = 32; selectedTile.Height = 32; //DEBUG

            level01 = new Level("level01", GraphicsDevice);

            #region // Towers //

            coordHandler = new CoordHandler(camera);
            contentManager = new ContentManager(this.Content);
            towerManager = new TowerManager(coordHandler, contentManager);

            #endregion

            base.Initialize();
        }

        protected override void LoadContent () {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            level01.Load(Content);

            selectTex = Content.Load<Texture2D>("Debug/select"); //DEBUG
        }

        protected override void UnloadContent () {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update (GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentState) {
                case GameStates.MENU: //vente med denne til slutt
                    break;

                case GameStates.INGAME:
                    var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    var keyboardState = Keyboard.GetState();

                    const float cameraSpeed = 500f;
                    const float zoomSpeed = 0.3f;

                    var moveDirection = Vector2.Zero;

                    if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                        moveDirection -= Vector2.UnitY;

                    if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                        moveDirection -= Vector2.UnitX;

                    if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                        moveDirection += Vector2.UnitY;

                    if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                        moveDirection += Vector2.UnitX;

                    if (moveDirection != Vector2.Zero) {
                        moveDirection.Normalize();
                        camera.Move(moveDirection * cameraSpeed * deltaSeconds);
                    }

                    if (keyboardState.IsKeyDown(Keys.R))
                        camera.ZoomIn(zoomSpeed * deltaSeconds);

                    if (keyboardState.IsKeyDown(Keys.F))
                        camera.ZoomOut(zoomSpeed * deltaSeconds);

                    selectedTile.X = (int)Math.Floor(camera.ScreenToWorld(Mouse.GetState().Position.X, Mouse.GetState().Position.Y).X / 32) * 32;
                    selectedTile.Y = (int)Math.Floor(camera.ScreenToWorld(Mouse.GetState().Position.X, Mouse.GetState().Position.Y).Y / 32) * 32;
                    selectedTile.Width = 32 * (int)camera.Zoom;
                    selectedTile.Height = 32 * (int)camera.Zoom;

                    level01.Update(gameTime);

                    #region // Towers //

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && lastTowerSpawn > towerSpawnCooldown) {
                        towerManager.SpawnTower(new MageTower(towerManager, contentManager), new CoordinateSystem.TileCoord(selectedTile.X, selectedTile.Y));
                        lastTowerSpawn = 0;
                    }

                    towerManager.Update(gameTime);

                    lastTowerSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    #endregion

                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            switch (currentState) {
                case GameStates.MENU: //vente med denne til slutt
                    break;

                case GameStates.INGAME:
                    level01.Draw(spriteBatch, camera, GraphicsDevice);

                    spriteBatch.Begin();
                    spriteBatch.Draw(selectTex, new Rectangle((int)camera.WorldToScreen(selectedTile.X, selectedTile.Y).X + 1, (int)camera.WorldToScreen(selectedTile.X, selectedTile.Y).Y + 1, 32 * (int)camera.Zoom, 32 * (int)camera.Zoom), Color.White);

                    #region // Towers //

                    towerManager.Draw(spriteBatch);

                    #endregion

                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
