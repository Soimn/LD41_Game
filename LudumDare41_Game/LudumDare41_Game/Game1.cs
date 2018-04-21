using LudumDare41_Game.UI;
using LudumDare41_Game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace LudumDare41_Game {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        enum GameStates { MENU, INGAME }; //gamestates, legg til om vi trenger
        GameStates currentState = GameStates.INGAME;

        private Camera2D camera;

        Level level01;

        GUI gui;

        SpriteFont debugFont;

        public Game1 () {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize () {
            camera = new Camera2D(GraphicsDevice) {
                Zoom = 2f
            };

            level01 = new Level("level01", GraphicsDevice);

            gui = new GUI(GraphicsDevice, Content);
            base.Initialize();
        }

        protected override void LoadContent () {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            level01.Load(Content);
            gui.Load();

            debugFont = Content.Load<SpriteFont>("GUI/Debug/debugFont");
            
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

                    level01.Update(gameTime);

                    gui.Update(gameTime, Window, camera);
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
                    level01.Draw(spriteBatch, camera, GraphicsDevice); //WORLD

                    spriteBatch.Begin(); //UI
                    gui.Draw(spriteBatch);

                    spriteBatch.DrawString(debugFont, "FPS: " + (Math.Round(1000/gameTime.ElapsedGameTime.TotalMilliseconds)).ToString(), new Vector2(0, 0), Color.Black); //FPS Counter
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
