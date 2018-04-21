using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.Graphics {

    public class Animation {
        public Texture2D spritesheet { get; private set; } //The spritesheet which the animation frames are laid out in.
        public Vector2 textureSize { get; private set; } //The size of the sprites. X = width, Y = height.
        public int frames { get; private set; } //Total frames in the animation. It will loop through these frames when drawing.
        public float speed { get; set; } //Speed of the animation in millis. A speed of 1000 would mean 1 second between every frame, 500 = 2hz etc.

        private int frameIndex = 0; //Which frame we're on.
        private float time = 0; //Tracking time elapsed.

        public Animation(Texture2D spritesheet, Vector2 imageSize, int totalFrames, float animationSpeed) { //Creating and assigning the appropriate variables.
            this.spritesheet = spritesheet;
            textureSize = imageSize;
            frames = totalFrames;
            speed = animationSpeed;
        }

        public void updateAnimation(GameTime gt) {
            time += (float)gt.ElapsedGameTime.TotalMilliseconds; //Add to the current time the elapsed time since the last update cycle. (60hz)

            if (time > speed) { //Increment current frame when time was reached.
                frameIndex++;
                time = 0;
            }

            if (frameIndex > (frames - 1)) //Loop when we reach the end. 
                frameIndex = 0;
        }

        public void drawAnimation(SpriteBatch sb, Vector2 pos) { //Draw the animation at the position specified w/ pos and in the SpriteBatch sb.
            Rectangle source = new Rectangle(Convert.ToInt16(frameIndex * textureSize.X), 0, (int)textureSize.X, (int)textureSize.Y);
            sb.Draw(spritesheet, new Rectangle((int)pos.X, (int)pos.Y, (int)textureSize.X * (int)Game1.camera.Zoom, (int)textureSize.Y * (int)Game1.camera.Zoom), source, Color.White);
        }

        public void drawAnimation (SpriteBatch sb, Vector2 pos, Color color) { //Draw the animation at the position specified w/ pos and in the SpriteBatch sb w/ color.
            Rectangle source = new Rectangle(Convert.ToInt16(frameIndex * textureSize.X), 0, (int)textureSize.X, (int)textureSize.Y);
            sb.Draw(spritesheet, new Rectangle((int)pos.X, (int)pos.Y, (int)textureSize.X * (int)Game1.camera.Zoom, (int)textureSize.Y * (int)Game1.camera.Zoom), source, color);
        }
    }
}
