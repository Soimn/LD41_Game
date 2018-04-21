using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.UI {
    interface IGUIElement {
        Rectangle pos { get; set; }
        Texture2D texture { get; set; }
        bool isLoaded { get; set; }
        string nameID { get; set; }

        void Load(ContentManager c);
        void Update(GameTime gt, GameWindow w);
        void Draw(SpriteBatch sb);
    }

    class GUI {
        GraphicsDevice _g;
        ContentManager _c;
        
        public GraphicsDevice g { get => _g; set => _g = value; }

        List<GUIElement> elements = new List<GUIElement>();

        CardSelector cardSelector;

        public GUI(GraphicsDevice g, ContentManager c) {
            _g = g;
            _c = c;

            cardSelector = new CardSelector("cardSel", Rectangle.Empty);
            addGuiItem(cardSelector);
        }

        public void addGuiItem(GUIElement elementToAdd) {
            elements.Add(elementToAdd);
            Load();
        }

        public void Load() {
            foreach(GUIElement e in elements) {
                if (!e.isLoaded) {
                    e.Load(_c);
                }
            }
        }

        public void Update(GameTime gt, GameWindow w) {
            foreach(GUIElement e in elements) {
                e.Update(gt, w);
                cardSelector.Update(gt, w);
            }
        }

        public void Draw(SpriteBatch sb) {
            foreach (GUIElement e in elements) {
                e.Draw(sb);
            }
        }
    }

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

    class CardSelector : GUIElement {

        int height = 200;
        int minHeight = 50;

        public CardSelector(string name, Rectangle pos) : base(name, pos) {
            nameID = name;
            this.pos = pos;
        }

        public new void Update(GameTime gt, GameWindow w) {
            if(Mouse.GetState().Position.Y > pos.Y) {
                pos = new Rectangle(0, w.ClientBounds.Height - height, w.ClientBounds.Width, height);
            }
            else {
                pos = new Rectangle(0, w.ClientBounds.Height - minHeight, w.ClientBounds.Width, minHeight);
            }
        }
    }
}
