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
            addGuiItem(waveInfo);

            cardSelector = new CardSelector("cardSel", Rectangle.Empty);
            addGuiItem(cardSelector);

            worldSelector = new WorldSelector("select", Rectangle.Empty);
            addGuiItem(worldSelector);

            manaHandler = new ManaHandler("manaHandler", Rectangle.Empty);
            addGuiItem(manaHandler);


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

            if (!waveInfo.isLoaded)
                waveInfo.Load(c);
        }

        public void Update(GameTime gt, GameWindow w, Camera2D camera) {
            foreach (GUIElement e in elements) {
                e.Update(gt, w);
            }

            cardSelector.Update(gt, w);
            worldSelector.Update(gt, w, camera);
            //manaHandler.Update(gt, w);
            waveInfo.Update(gt, w);
        }

        public void Draw(SpriteBatch sb) {
            foreach (GUIElement e in elements) {
                if(e.nameID != "waveInfo")
                    e.Draw(sb);
            }

            manaHandler.Draw(sb);
            waveInfo.Draw(sb);
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

        public CardSelector(string name, Rectangle pos) : base(name, pos) {
            nameID = name;
            this.pos = pos;
        }

        public new void Update(GameTime gt, GameWindow w) {
            if (Mouse.GetState().Position.Y > pos.Y) {
                pos = new Rectangle(0, w.ClientBounds.Height - height, w.ClientBounds.Width, height);
                isActive = true;
            }
            else {
                if (Mouse.GetState().LeftButton.Equals(ButtonState.Released)) {
                    pos = new Rectangle(0, w.ClientBounds.Height - minHeight, w.ClientBounds.Width, minHeight);
                    isActive = false;
                }
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

            pos = new Rectangle((int)camera.WorldToScreen(selectedTile.X, selectedTile.Y).X + 1, (int)camera.WorldToScreen(selectedTile.X, selectedTile.Y).Y + 1, 32 * (int)camera.Zoom, 32 * (int)camera.Zoom);
        }
    }

    class ManaHandler : GUIElement {
        public ManaHandler(string name, Rectangle pos) : base(name, pos) {
        }

        /*public new void Update(GameTime gt, GameWindow w) {
        } use later? */

        public new void Draw(SpriteBatch sb) {
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent, new Vector2(10 + 1, 15), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent, new Vector2(10, 15 + 1), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent, new Vector2(10 - 1, 15), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent, new Vector2(10, 15 - 1), Color.Black);
            sb.DrawString(Card.healthFont, "Mana: " + Cards.manaCurrent, new Vector2(10, 15), Color.White);
        }
    }

    class WaveInfo : GUIElement {
        Animation waveCountdown;
        bool oldRoundState;
        bool showCountdown = false;

        int countdown = 10;

        public WaveInfo(string name, Rectangle pos) : base(name, pos) {
        }

        public new void Load(ContentManager c) {
            waveCountdown = new Animation(c.Load<Texture2D>("GUI/WaveCountdown"), new Vector2(32, 64), 4, 500);
        }
        
        public new void Update(GameTime gt, GameWindow w, WaveManager waveManager) {
            bool newRoundState = waveManager.IsWaveOngoing();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                newRoundState = true;

            if (newRoundState && !oldRoundState) {
                waveCountdown.ResetAnimation();
                showCountdown = true;
            }

            if(!newRoundState && oldRoundState) {
                showCountdown = false;
            }

            if (showCountdown) {
                waveCountdown.updateAnimation(gt);
                countdown = waveManager.SecondsTillNextWave();
            }

            pos = new Rectangle((w.ClientBounds.Width / 2) - 32, 80, 32, 64);

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
        }

    }
    #endregion
}
