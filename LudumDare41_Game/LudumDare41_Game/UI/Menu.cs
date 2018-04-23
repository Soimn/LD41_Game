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
        Texture2D title;

        public Menu() {

        }

        public void Load(ContentManager c) {
            title = c.Load<Texture2D>("GUI/Menu/title");
        }

        public void Update(GameTime gt) {
            if(Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                Game1.currentState = Game1.GameStates.INGAME;
            }
        }

        public void Draw(SpriteBatch sb, GameWindow w) {
            sb.Begin();

            sb.Draw(title, new Rectangle((w.ClientBounds.Width / 2) - 200, 10, 400, 300), Color.White);

            sb.End();
        }

    }
}
