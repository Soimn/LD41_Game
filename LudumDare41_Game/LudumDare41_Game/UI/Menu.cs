using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.UI {
    class Menu {
        Texture2D title, play, playSelect, exit, exitSelect;
        Rectangle mouseRect, playRect, exitRect;
        Song menuSong;

        public Menu() {

        }

        public void Load(ContentManager c) {
            title = c.Load<Texture2D>("GUI/Menu/title");

            play = c.Load<Texture2D>("GUI/Menu/play");
            playSelect = c.Load<Texture2D>("GUI/Menu/playSelect");

            exit = c.Load<Texture2D>("GUI/Menu/exit");
            exitSelect = c.Load<Texture2D>("GUI/Menu/exitSelect");

            menuSong = c.Load<Song>("Audio/mainSong");

            MediaPlayer.Play(menuSong);
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.IsRepeating = true;
        }

        public void Update(GameTime gt, Game1 game) {
            mouseRect = new Rectangle((int)Mouse.GetState().Position.X, (int)Mouse.GetState().Position.Y, 1, 1);

            if (mouseRect.Intersects(playRect) 
                && Mouse.GetState().LeftButton.Equals(ButtonState.Pressed)) {
                Game1.currentState = Game1.GameStates.INGAME;
            }
            if (mouseRect.Intersects(exitRect)
                && Mouse.GetState().LeftButton.Equals(ButtonState.Pressed)) {
                game.Exit();
            }

            

        }

        public void Draw(SpriteBatch sb, GameWindow w) {
            Texture2D playBtn, exitBtn;
            playRect = new Rectangle((w.ClientBounds.Width / 2) - 200, 350, play.Width, play.Height);
            exitRect = new Rectangle((w.ClientBounds.Width / 2) - 200, 450, exit.Width, exit.Height);

            sb.Begin(samplerState: SamplerState.PointWrap);
            sb.Draw(title, new Rectangle((w.ClientBounds.Width / 2) - 200, 10, 400, 300), Color.White);

            if (mouseRect.Intersects(playRect)) {
                playBtn = playSelect;
            }
            else {
                playBtn = play;
            }

            if (mouseRect.Intersects(exitRect)) {
                exitBtn = exitSelect;
            }
            else {
                exitBtn = exit;
            }

            sb.Draw(playBtn, playRect, Color.White);
            sb.Draw(exitBtn, exitRect, Color.White);

            sb.End();
        }
    }
}
