using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        CardSelector cardSelector;
        WorldSelector worldSelector;

        public GUI(GraphicsDevice g, ContentManager c) {
            this.g = g;
            this.c = c;

            cardSelector = new CardSelector("cardSel", Rectangle.Empty);
            addGuiItem(cardSelector);

            worldSelector = new WorldSelector("select", Rectangle.Empty);
            addGuiItem(worldSelector);
            
        }

        public void addGuiItem(GUIElement elementToAdd) {
            elements.Add(elementToAdd);
            Load();
        }

        public void Load() {
            foreach (GUIElement e in elements) {
                if (!e.isLoaded) {
                    e.Load(c);
                }
            }
        }

        public void Update(GameTime gt, GameWindow w, Camera2D camera) {
            foreach (GUIElement e in elements) {
                e.Update(gt, w);
                cardSelector.Update(gt, w);
                worldSelector.Update(gt, w, camera);
            }
        }

        public void Draw(SpriteBatch sb) {
            foreach (GUIElement e in elements) {
                e.Draw(sb);
            }
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

        int height = 200;
        int minHeight = 50;

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
    #endregion
}
