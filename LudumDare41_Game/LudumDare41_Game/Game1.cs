using LudumDare41_Game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LudumDare41_Game.Physics {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        enum GameStates { MENU, INGAME }; //gamestates, legg til om vi trenger
        GameStates currentState = GameStates.INGAME;

        private Camera2D camera;

        Level level01;

        public Game1 () {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize () {
            camera = new Camera2D(GraphicsDevice);
            camera.Zoom = 2f;

            level01 = new Level("level01", GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent () {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            level01.Load(Content);
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

                    var isCameraMoving = moveDirection != Vector2.Zero;
                    if (isCameraMoving) {
                        moveDirection.Normalize();
                        camera.Move(moveDirection * cameraSpeed * deltaSeconds);
                    }

                    if (keyboardState.IsKeyDown(Keys.R))
                        camera.ZoomIn(zoomSpeed * deltaSeconds);

                    if (keyboardState.IsKeyDown(Keys.F))
                        camera.ZoomOut(zoomSpeed * deltaSeconds);


                    level01.Update(gameTime);
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
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
