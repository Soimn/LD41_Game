using  LudumDare41_Game.UI;
using LudumDare41_Game.Content;
using LudumDare41_Game.Towers;
using LudumDare41_Game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using LudumDare41_Game.Physics;

namespace LudumDare41_Game {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { MENU, INGAME, PAUSE }; //gamestates, legg til om vi trenger
        GameStates currentState = GameStates.INGAME;

        private Camera2D camera;

        Level level01;
      
        GUI gui;
        Cards cards;

        SpriteFont debugFont;

        #region // Towers //

        private TowerManager towerManager;
        private ContentManager contentManager;
        private CoordHandler coordHandler;

        private bool cardHasBeenHeld = false;
        private bool previewTowerInstantiated = false;
        private Tower previewTower;

        #endregion


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
            cards = new Cards();

            Window.Title = "Ludum Dare 41: Card game tower defence";

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
            gui.Load();
            cards.Load(Content);

            debugFont = Content.Load<SpriteFont>("GUI/Debug/debugFont");
            
        }

        protected override void UnloadContent () {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update (GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
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

                    if (moveDirection != Vector2.Zero) {
                        moveDirection.Normalize();
                        camera.Move(moveDirection * cameraSpeed * deltaSeconds);
                    }

                    level01.Update(gameTime);


                    gui.Update(gameTime, Window, camera);
                    cards.Update(gameTime, Window);

                    #region // Towers //

                    HandCard heldCard = cards.CurrentlyHeldCard();

                    if (heldCard != null && !cardHasBeenHeld && !previewTowerInstantiated) {
                        cardHasBeenHeld = true;
                        towerManager.CreatePreviewTower(heldCard.referenceCard.TowerID, new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y), out previewTower);
                        previewTowerInstantiated = true;
                    }

                    else if (heldCard != null && previewTowerInstantiated)
                        previewTower.MoveTo(new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y));

                    else if (heldCard == null && cardHasBeenHeld) {
                        if (!towerManager.TowerAtCoord(new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y))) {
                            towerManager.DestroyPreviewTower(previewTower);
                            previewTowerInstantiated = false;
                            previewTower = null;
                            towerManager.SpawnTower(new MageTower(towerManager, contentManager), new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y));
                            cardHasBeenHeld = false;

                            // remove card from hand
                        }
                        else {
                            towerManager.DestroyPreviewTower(previewTower);
                            previewTowerInstantiated = false;
                            previewTower = null;
                            cardHasBeenHeld = false; // return card to hand
                        }
                    }

                    towerManager.Update(gameTime);

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
                    level01.Draw(spriteBatch, camera, GraphicsDevice); //WORLD

                    spriteBatch.Begin();

                    #region // Towers //

                    towerManager.Draw(spriteBatch);

                    #endregion

                    spriteBatch.End();

                    spriteBatch.Begin(); //UI
                    gui.Draw(spriteBatch);
                    cards.Draw(spriteBatch, Window);


                    spriteBatch.DrawString(debugFont, "FPS: " + (Math.Round(1000/gameTime.ElapsedGameTime.TotalMilliseconds)).ToString(), new Vector2(0, 0), Color.Black); //FPS Counter

                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
