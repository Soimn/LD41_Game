using LudumDare41_Game.Entities;
using LudumDare41_Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.UI {
    #region Gui Element Interface
    interface IGUIElement {
        Rectangle pos { get; set; }
        Texture2D texture { get; set; }
        bool isLoaded { get; set; }
        string nameID { get; set; }

        void Load(ContentManager c);
        void Update(GameTime gt, GameWindow w);
        void Draw(SpriteBatch sb);
    }
    #endregion

    #region GUI Class
    class GUI {
        GraphicsDevice g { get; set; }
        ContentManager c;

        List<GUIElement> elements = new List<GUIElement>();

        public CardSelector cardSelector { get; private set; }
        public WorldSelector worldSelector { get; private set; }
        public ManaHandler manaHandler { get; private set; }
        public WaveInfo waveInfo { get; private set; }

        public GUI(GraphicsDevice g, ContentManager c) {
            this.g = g;
            this.c = c;

            waveInfo = new WaveInfo("waveInfo", Rectangle.Empty);

            cardSelector = new CardSelector("cardSel", new Rectangle(1000, 1000, 0, 0));

            worldSelector = new WorldSelector("select", Rectangle.Empty);

            manaHandler = new ManaHandler("manaHandler", Rectangle.Empty);
            addGuiItem(manaHandler);
            addGuiItem(waveInfo);
            addGuiItem(cardSelector);
            addGuiItem(worldSelector);

        }

        public void addGuiItem(GUIElement elementToAdd) {
            elements.Add(elementToAdd);
            Load();
        }

        public void Load() {
            foreach (GUIElement e in elements) {
                if (!e.isLoaded && e.nameID != "waveInfo") {
                    e.Load(c);
                }
            }
            
            waveInfo.Load(c);
            
            cardSelector.Load(c);

            manaHandler.Load(c);
        }

        public void Update(GameTime gt, GameWindow w, Camera2D camera, WaveManager wave) {
            foreach (GUIElement e in elements) {
                e.Update(gt, w);
            }

            cardSelector.Update(gt, w);
            worldSelector.Update(gt, w, camera);
            //manaHandler.Update(gt, w);
            waveInfo.ActualUpdate(gt, w, wave);
        }

        public void Draw(SpriteBatch sb, GameWindow w) {
            foreach (GUIElement e in elements) {
                if(e.nameID != "waveInfo")
                    e.Draw(sb);
            }

            manaHandler.Draw(sb);
            waveInfo.Draw(sb);
            cardSelector.DrawTutorial(sb, w);
        }
    }
    #endregion

    #region GUI Element
    class GUIElement : IGUIElement {
        Rectangle _pos;
        bool _isLoaded;
        Texture2D _texture;
        string _nameID;

        public Rectangle pos { get => _pos; set => _pos = value; }
        public bool isLoaded { get => _isLoaded; set => _isLoaded = value; }
        public Texture2D texture { get => _texture; set => _texture = value; }

        public string nameID { get => _nameID; set => _nameID = value; }

        public GUIElement(string name, Rectangle pos) {
            _pos = pos;
            _nameID = name;
            _isLoaded = false;
        }

        public void Load(ContentManager c) {
            _texture = c.Load<Texture2D>("GUI/" + _nameID);
            _isLoaded = true;
        }

        public void Update(GameTime gt, GameWindow w) { }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, pos, Color.White);
        }
    }
    #endregion

    #region GUI Classes
    class CardSelector : GUIElement {
        int minHeight = 50;
        int height = 200;

        public static bool isActive = false;

        Texture2D cardSelTutorial, cardDrawTutorial, drawCard, drawCardSel, drawCardNoMana, drawCardCurrent;
        Rectangle mouseRect, drawCardButton;

        MouseState old;

        public CardSelector(string name, Rectangle pos) : base(name, pos) {
            nameID = name;
            this.pos = pos;
        }

        public new void Load(ContentManager c) {
            cardSelTutorial = c.Load<Texture2D>("Tutorial/CardSel");
            cardDrawTutorial = c.Load<Texture2D>("Tutorial/drawCard");
            drawCard = c.Load<Texture2D>("GUI/Cards/drawCard");
            drawCardSel = c.Load<Texture2D>("GUI/Cards/drawCardSel");
            drawCardNoMana = c.Load<Texture2D>("GUI/Cards/drawCardSelNoMana");
        }

        public new void Update(GameTime gt, GameWindow w) {
            MouseState New = Mouse.GetState();
            if (Mouse.GetState().Position.Y > pos.Y) {

                pos = new Rectangle(0, w.ClientBounds.Height - height, w.ClientBounds.Width, height);
                isActive = true;

                mouseRect = new Rectangle((int)Mouse.GetState().Position.X, (int)Mouse.GetState().Position.Y, 1, 1);
                drawCardButton = new Rectangle(w.ClientBounds.Width - 128, w.ClientBounds.Height - 128, 64, 64);

                if (mouseRect.Intersects(drawCardButton)) {
                    if(Cards.manaCurrent >= Cards.manaCostCardDraw) {
                        drawCardCurrent = drawCardSel;
                        if (New.LeftButton.Equals(ButtonState.Pressed) && old.LeftButton.Equals(ButtonState.Released)) {
                            CardDraw.DrawCard(1);
                            Cards.manaCurrent -= Cards.manaCostCardDraw;
                        }
                    }
                    else {
                        drawCardCurrent = drawCardNoMana;
                    }
                }
                else {
                    drawCardCurrent = drawCard;
                }
            }
            else {
                if (Mouse.GetState().LeftButton.Equals(ButtonState.Released)) {
                    pos = new Rectangle(0, w.ClientBounds.Height - minHeight, w.ClientBounds.Width, minHeight);
                    isActive = false;
                }
            }
            old = New;
        }

        public void DrawTutorial(SpriteBatch sb, GameWindow w) {
            if (Game1.isTutorial) {
                if (!CardSelector.isActive) { 
                    sb.Draw(cardSelTutorial, new Rectangle((w.ClientBounds.Width / 2) - (cardSelTutorial.Width / 2) - 25, pos.Y - 80, cardSelTutorial.Width, cardSelTutorial.Height), Color.White);
                }
                else {
                    sb.Draw(cardDrawTutorial, new Rectangle(drawCardButton.X - 220, drawCardButton.Y - 155, cardDrawTutorial.Width, cardDrawTutorial.Height), Color.White);
                }
            }
                
            if (isActive) {
                sb.DrawString(Card.healthFont, "Draw a card", new Vector2(drawCardButton.X + (drawCardCurrent.Width / 2) - (Card.healthFont.MeasureString("Draw a card").X / 2), drawCardButton.Y - 30), Color.White);
                sb.Draw(drawCardCurrent, drawCardButton, Color.White);
                if(Cards.manaCurrent < Cards.manaCostCardDraw)
                    sb.DrawString(Card.healthFont, "Not enough \nmana", new Vector2(drawCardButton.X + (drawCardCurrent.Width / 2) - (Card.healthFont.MeasureString("Draw a card").X / 2), drawCardButton.Y + 60), Color.Red);

            }
        }
    }

    class WorldSelector : GUIElement {

        public static Rectangle selectedTile;

        public WorldSelector(string name, Rectangle pos) : base(name, pos) {
            nameID = name;
            this.pos = pos;
        }

        public void Update(GameTime gt, GameWindow w, Camera2D camera) {
            selectedTile.X = (int)Math.Floor(camera.ScreenToWorld(Mouse.GetState().Position.X, Mouse.GetState().Position.Y).X / 32) * 32;
            selectedTile.Y = (int)Math.Floor(camera.ScreenToWorld(Mouse.GetState().Position.X, Mouse.GetState().Position.Y).Y / 32) * 32;
            selectedTile.Width = 32 * (int)camera.Zoom;
            selectedTile.Height = 32 * (int)camera.Zoom;

            if (!CardSelector.isActive) {
                pos = new Rectangle((int)camera.WorldToScreen(selectedTile.X, selectedTile.Y).X + 1, (int)camera.WorldToScreen(selectedTile.X, selectedTile.Y).Y + 1, 32 * (int)camera.Zoom, 32 * (int)camera.Zoom);
            }
            else {
                pos = new Rectangle(0, 0, 0, 0);
            }
                
        }
    }

    class ManaHandler : GUIElement {
        Texture2D manaTutorial;

        public ManaHandler(string name, Rectangle pos) : base(name, pos) {
        }

        public new void Load(ContentManager c) {
            manaTutorial = c.Load<Texture2D>("Tutorial/mana");
            
        }

        /*public new void Update(GameTime gt, GameWindow w) {
        } use later? */

        public new void Draw(SpriteBatch sb) {
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent + "/" + Cards.manaCostCardDraw, new Vector2(10 + 1, 15), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent + "/" + Cards.manaCostCardDraw, new Vector2(10, 15 + 1), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent + "/" + Cards.manaCostCardDraw, new Vector2(10 - 1, 15), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent + "/" + Cards.manaCostCardDraw, new Vector2(10, 15 - 1), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent + "/" + Cards.manaCostCardDraw, new Vector2(10, 15), Color.White);

            if (Game1.isTutorial)
                sb.Draw(manaTutorial, new Rectangle(115, 45, manaTutorial.Width, manaTutorial.Height), Color.White);
        }
    }

    class WaveInfo : GUIElement {
        Animation waveCountdown;
        bool oldRoundState;
        bool showCountdown = false;

        Texture2D waveCounterTutorial;

        int countdown = 10; //hent fra Simon <- her skal jeg ha hvor lang tid det er imellom waves.

        public WaveInfo(string name, Rectangle pos) : base(name, pos) {
        }

        public new void Load(ContentManager c) {
            waveCountdown = new Animation(c.Load<Texture2D>("GUI/WaveCountdown"), new Vector2(32, 64), 4, 500);
            waveCounterTutorial = c.Load<Texture2D>("Tutorial/Waves");
        }
        
        public void ActualUpdate(GameTime gt, GameWindow w, WaveManager waveManager) {
            bool newRoundState = !waveManager.IsWaveOngoing();

            if (!Game1.isTutorial) {
                if (newRoundState && !oldRoundState) {
                    waveCountdown.ResetAnimation();
                    showCountdown = true;
                }

                if (!newRoundState && oldRoundState) {
                    showCountdown = false;
                }

                if (showCountdown) {
                    waveCountdown.updateAnimation(gt);
                    countdown = waveManager.SecondsTillNextWave();
                }
            } else {
                showCountdown = true;
                newRoundState = true;
            }

            pos = new Rectangle((int)Game1.camera.WorldToScreen(320, 80).X, (int)Game1.camera.WorldToScreen(400, 80).Y, 32, 64);
            oldRoundState = newRoundState;
        }

        public new void Draw(SpriteBatch sb) {
            if (showCountdown) {
                waveCountdown.drawAnimation(sb, new Vector2(pos.X, pos.Y), Color.White);
                if(countdown > 9) {
                    sb.DrawString(Card.healthFont, countdown.ToString(), new Vector2(pos.X + 45 - Card.healthFont.MeasureString(countdown.ToString()).X, pos.Y + 10), Color.White);
                }
                else {
                    sb.DrawString(Card.healthFont, countdown.ToString(), new Vector2(pos.X + 42 - Card.healthFont.MeasureString(countdown.ToString()).X, pos.Y + 10), Color.White);
                }
            }

            if (Game1.isTutorial)
                sb.Draw(waveCounterTutorial, new Rectangle(pos.X - 17, pos.Y - 70, waveCounterTutorial.Width, waveCounterTutorial.Height), Color.White);
        }
    }
    #endregion
}
