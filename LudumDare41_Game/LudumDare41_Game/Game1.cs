using LudumDare41_Game.UI;
using LudumDare41_Game.Content;
using LudumDare41_Game.Towers;
using LudumDare41_Game.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using LudumDare41_Game.Physics;
using LudumDare41_Game.Entities;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework.Media;

namespace LudumDare41_Game {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState oldState;

        Texture2D tutorial;

        public enum GameStates { MENU, INGAME }; //gamestates, legg til om vi trenger
        bool isPaused = false;
        public static GameStates currentState = GameStates.MENU;
        GameStates lastState;

        public static Camera2D camera { get; private set; }
        public static bool isTutorial { get; set; }
        public static bool isGameOver { get; set; }

        private static Level level01;
        internal static Level Level01 { get => level01; }

        GUI gui;
        Cards cards;

        SpriteFont debugFont;

        Home home;

        Menu menu;

        #region // Towers //

        private TowerManager towerManager;
        private ContentManager contentManager;
        private CoordHandler coordHandler;
        private EntityManager entityManager;

        private bool cardHasBeenHeld = false;
        private bool previewTowerInstantiated = false;
        private Tower previewTower;

        #endregion

        private WaveManager waveManager;


        Texture2D tutorialStart;

        public bool DebugMode { get; private set; }

        private float lastTime, cooldown = 0.5f;

        private HandCard previousHeldCard;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize() {
            camera = new Camera2D(GraphicsDevice) {
                Zoom = 2f
            };

            level01 = new Level("level01", GraphicsDevice);

            gui = new GUI(GraphicsDevice, Content);
            cards = new Cards();


            Window.Title = "God does play cards!";

            #region // Towers //

            coordHandler = new CoordHandler(camera);
            contentManager = new ContentManager(Content);
            waveManager = new WaveManager();
            entityManager = new EntityManager(coordHandler, contentManager, waveManager);
            towerManager = new TowerManager(coordHandler, contentManager, entityManager, this);
            waveManager.Init(entityManager);

            #endregion

            menu = new Menu();

            isTutorial = true;
            tutorial = Content.Load<Texture2D>("GUI/cardSel");
            tutorialStart = Content.Load<Texture2D>("Tutorial/start");

            isGameOver = false;

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            level01.Load(Content);
            gui.Load();
            cards.Load(Content);

            home = new Home(new Vector2(19, 31), Content);

            debugFont = Content.Load<SpriteFont>("GUI/Debug/debugFont");

            menu.Load(Content);
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update (GameTime gameTime) {

            KeyboardState newState = Keyboard.GetState();
            GameStates newGameState = currentState;

            if (!isPaused && !isGameOver) {
                switch (currentState) {
                    case GameStates.MENU: //vente med denne til slutt
                        menu.Update(gameTime, this);
                        break;

                    case GameStates.INGAME:
                        if (!isTutorial)
                            waveManager.Update(gameTime);

                        if (newGameState == GameStates.INGAME && lastState == GameStates.MENU) {
                            MediaPlayer.Stop();
                        }

                        level01.Update(gameTime);

                        gui.Update(gameTime, Window, camera, waveManager);
                        cards.Update(gameTime, Window, isTutorial);

                        home.Update(gameTime);

                        if (!isTutorial) {
                            if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape)) {
                                if (isPaused) {
                                    isPaused = false;
                                }
                                else {
                                    isPaused = true;
                                }
                            }

                            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            var keyboardState = Keyboard.GetState();

                            const float cameraSpeed = 500f;

                            var moveDirection = Vector2.Zero;


                            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) {
                                if (!(camera.ScreenToWorld(0f, 0f).Y < Vector2.Zero.Y)) {
                                    moveDirection -= Vector2.UnitY;
                                }
                            }

                            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) {
                                if (!(camera.ScreenToWorld(Window.ClientBounds.Width, Window.ClientBounds.Height).Y > level01.map.HeightInPixels)) {
                                    moveDirection += Vector2.UnitY;
                                }
                            }

                            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) {
                                if (!(camera.ScreenToWorld(0, 0).X < Vector2.Zero.X)) {
                                    moveDirection -= Vector2.UnitX;
                                }
                            }

                            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)) {
                                if (!(camera.ScreenToWorld(Window.ClientBounds.Width, Window.ClientBounds.Height).X > level01.map.WidthInPixels)) {
                                    moveDirection += Vector2.UnitX;
                                }
                            }

                            if ((camera.ScreenToWorld(Window.ClientBounds.Width, Window.ClientBounds.Height).X > level01.map.WidthInPixels + 10)) {
                                camera.Move(new Vector2(-1, 0) * 10000 * deltaSeconds); 
                            }

                            if ((camera.ScreenToWorld(0, 0).X < Vector2.Zero.X - 10)) {
                                camera.Move(new Vector2(1, 0) * 10000 * deltaSeconds);
                            }

                            if ((camera.ScreenToWorld(Window.ClientBounds.Width, Window.ClientBounds.Height).Y > level01.map.HeightInPixels + 10)) {
                                camera.Move(new Vector2(0, -1) * 10000 * deltaSeconds);
                            }
                            

                            var isCameraMoving = moveDirection != Vector2.Zero;
                            if (isCameraMoving) {
                                moveDirection.Normalize();
                                camera.Move(moveDirection * cameraSpeed * deltaSeconds);
                            }
                            

                            #region // Towers //

                            HandCard heldCard = cards.CurrentlyHeldCard();

                            if (heldCard != null && !cardHasBeenHeld && !previewTowerInstantiated) {
                                cardHasBeenHeld = true;
                                towerManager.CreatePreviewTower(heldCard.referenceCard.TowerID, new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y), out previewTower);
                                previewTowerInstantiated = true;
                                previousHeldCard = heldCard;
                            }

                            else if (heldCard != null && previewTowerInstantiated)
                                previewTower.MoveTo(new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y));

                            else if (heldCard == null && cardHasBeenHeld) {
                                if (!towerManager.InvalidCoord(new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y))) {
                                    towerManager.DestroyPreviewTower(previewTower);
                                    previewTowerInstantiated = false;
                                    previewTower = null;
                                    towerManager.SpawnTower(previousHeldCard.referenceCard.TowerID, new CoordinateSystem.TileCoord(UI.WorldSelector.selectedTile.X, UI.WorldSelector.selectedTile.Y));
                                    cardHasBeenHeld = false;
                                    previousHeldCard = null;

                                    Cards.cardsInHand.Remove(Cards.previouslyHeldCard);
                                }
                                else {
                                    towerManager.DestroyPreviewTower(previewTower);
                                    previewTowerInstantiated = false;
                                    previewTower = null;
                                    cardHasBeenHeld = false; // return card to hand
                                }
                            }

                            towerManager.Update(gameTime);
                            entityManager.Update(gameTime);


                            #endregion

                            if (Mouse.GetState().RightButton.Equals(ButtonState.Pressed) && cardHasBeenHeld) {
                                towerManager.DestroyPreviewTower(previewTower);
                                previewTowerInstantiated = false;
                                previewTower = null;
                                cardHasBeenHeld = false; // return card to hand
                                Cards.cardsInHand.Remove(Cards.previouslyHeldCard);
                                Cards.returnToHand = true;
                                Cards.anyHeld = false;
                                Cards.heldCard = null;
                            }
                            else if (Cards.returnToHand
                              && Mouse.GetState().LeftButton.Equals(ButtonState.Released)) {
                                Cards.cardsInHand.Add(Cards.previouslyHeldCard);
                                Cards.returnToHand = false;
                            }

                        }
                        else {
                            if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape)) {
                                isTutorial = false;
                            }

                            camera.Position = new Vector2(-320, -150);
                        }
                        break;
                }
            }
            else {
                if (!isGameOver) {
                    if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape)) {
                        if (isPaused) {
                            isPaused = false;
                        }
                        else {
                            isPaused = true;
                        }
                    }
                }
                else {

                }
                
            }

            lastState = newGameState;
            oldState = newState;

            lastTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.CapsLock) && lastTime > cooldown) {
                DebugMode = !DebugMode;
                lastTime = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(new Color(133, 167, 94));
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            switch (currentState) {
                case GameStates.MENU:
                    menu.Draw(spriteBatch, Window);
                    break;

                case GameStates.INGAME:
                    //WORLD
                    level01.Draw(spriteBatch, camera, GraphicsDevice);
                    spriteBatch.Begin(samplerState: SamplerState.PointWrap);
                    #region // Towers //

                    towerManager.Draw(spriteBatch);
                    entityManager.Draw(spriteBatch);

                    #endregion
                    spriteBatch.End();

                    //UI
                    spriteBatch.Begin(samplerState: SamplerState.PointWrap);
                    if (isTutorial)
                        spriteBatch.Draw(tutorialStart, new Rectangle((Window.ClientBounds.Width / 2) - (tutorialStart.Width / 2), (Window.ClientBounds.Height / 2) - (tutorialStart.Height / 2), tutorialStart.Width, tutorialStart.Height), Color.White);

                    home.Draw(spriteBatch);
                    gui.Draw(spriteBatch, Window);
                    cards.Draw(spriteBatch, Window);

                    if (isGameOver) {
                        spriteBatch.DrawString(Card.titleFont, "Game over!", new Vector2((Window.ClientBounds.Width / 2) - Card.titleFont.MeasureString("Game over!").X + 1, (Window.ClientBounds.Height / 2) - Card.titleFont.MeasureString("Game over!").Y + 1), Color.Black);
                        spriteBatch.DrawString(Card.titleFont, "Game over!", new Vector2((Window.ClientBounds.Width / 2) - Card.titleFont.MeasureString("Game over!").X - 1, (Window.ClientBounds.Height / 2) - Card.titleFont.MeasureString("Game over!").Y - 1), Color.Black);
                        spriteBatch.DrawString(Card.titleFont, "Game over!", new Vector2((Window.ClientBounds.Width / 2) - Card.titleFont.MeasureString("Game over!").X, (Window.ClientBounds.Height / 2) - Card.titleFont.MeasureString("Game over!").Y), Color.White);
                    }

                    #region // DEBUG //
                    spriteBatch.DrawString(debugFont, "FPS: " + (Math.Round(1000 / gameTime.ElapsedGameTime.TotalMilliseconds)).ToString(), new Vector2(0, 0), Color.Black); //FPS Counter

                    if (isPaused) {
                        string pausedMsg = "Game paused, press ESC to resume.";
                        spriteBatch.DrawString(debugFont, pausedMsg, new Vector2(Window.ClientBounds.Width / 2 - debugFont.MeasureString(pausedMsg).X / 2, Window.ClientBounds.Height / 2), Color.Black);
                    }


                    spriteBatch.End();
                    #endregion
                    break;
            }

            base.Draw(gameTime);
        }

        private void LookAtMapCenter() {
            switch (level01.map.Orientation) {
                case TiledMapOrientation.Orthogonal:
                    camera.LookAt(new Vector2(level01.map.WidthInPixels, level01.map.HeightInPixels) * 0.5f);
                    break;
            }
        }
    }
}