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
    class Menu {
        Texture2D title, play, playSelect;
        Rectangle mouseRect, playRect;

        public Menu() {

        }

        public void Load(ContentManager c) {
            title = c.Load<Texture2D>("GUI/Menu/title");
            play = c.Load<Texture2D>("GUI/Menu/play");
            playSelect = c.Load<Texture2D>("GUI/Menu/playSelect");
        }

        public void Update(GameTime gt) {
            mouseRect = new Rectangle((int)Mouse.GetState().Position.X, (int)Mouse.GetState().Position.Y, 1, 1);

            if (mouseRect.Intersects(playRect) 
                && Mouse.GetState().LeftButton.Equals(ButtonState.Pressed)) {
                Game1.currentState = Game1.GameStates.INGAME;
            }
        }

        public void Draw(SpriteBatch sb, GameWindow w) {
            Texture2D playBtn;
            playRect = new Rectangle((w.ClientBounds.Width / 2) - 200, 350, play.Width, play.Height);

            sb.Begin();
            sb.Draw(title, new Rectangle((w.ClientBounds.Width / 2) - 200, 10, 400, 300), Color.White);

            if (mouseRect.Intersects(playRect)) {
                playBtn = playSelect;
            }
            else {
                playBtn = play;
            }

            sb.Draw(playBtn, playRect, Color.White);
            sb.Draw(playBtn, playRect, Color.White);

            sb.End();
        }

    }
}
