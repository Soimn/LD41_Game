using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.Graphics {

    public class Animation {
        private static Texture2D _spritesheet; //The spritesheet which the animation frames are laid out in.
        private static Vector2 _textureSize; //The size of the sprites. X = width, Y = height.
        private static int _frames; //Total frames in the animation. It will loop through these frames when drawing.
        private static float _speed; //Speed of the animation in millis. A speed of 1000 would mean 1 second between every frame, 500 = 2hz etc.

        private int frameIndex = 0; //Which frame we're on.
        private float time = 0; //Tracking time elapsed.

        public Animation(Texture2D spritesheet, Vector2 imageSize, int totalFrames, float animationSpeed) { //Creating and assigning the appropriate variables.
            _spritesheet = spritesheet;
            _textureSize = imageSize;
            _frames = totalFrames;
            _speed = animationSpeed;
        }

        public void updateAnimation(GameTime gt) {
            time += (float)gt.ElapsedGameTime.TotalMilliseconds; //Add to the current time the elapsed time since the last update cycle. (60hz)

            if (time > _speed) { //Increment current frame when time was reached.
                frameIndex++;
                time = 0;
            }

            if (frameIndex > (_frames - 1)) //Loop when we reach the end. 
                frameIndex = 0;
        }

        public void drawAnimation(SpriteBatch sb, Vector2 pos) { //Draw the animation at the position specified w/ pos and in the SpriteBatch sb.
            Rectangle source = new Rectangle(Convert.ToInt16(frameIndex * _textureSize.X), 0, (int)_textureSize.X, (int)_textureSize.Y);
            sb.Draw(_spritesheet, new Rectangle((int)pos.X, (int)pos.Y, (int)_textureSize.X, (int)_textureSize.Y), source, Color.White);
        }

        public static float speed { get => _speed; set => _speed = value; }
        public static int frames { get => _frames; set => _frames = value; }
        public static Texture2D spritesheet { get => _spritesheet; set => _spritesheet = value; }
        public static Vector2 textureSize { get => _textureSize; set => _textureSize = value; }
    }
}
