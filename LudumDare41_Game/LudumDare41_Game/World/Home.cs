using LudumDare41_Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.World {
    class Home {
        public static int health { get; set; }
        public static Vector2 position { get; private set; }

        enum AnimationState { Idle, Hurt };
        AnimationState currentAnimation { get; set; }

        private ContentManager content;
        private Animation idleAnimation, hurtAnimation;

        public Home(Vector2 _position, ContentManager _content) {
            content = _content;
            position = _position;

            currentAnimation = AnimationState.Idle;
            idleAnimation = new Animation(_content.Load<Texture2D>("Entities/Home/gate"), new Vector2(192, 128), 3, 750);

            health = 200;
        }

        public void Update(GameTime gt) {
            switch (currentAnimation) {
                case AnimationState.Idle:
                    idleAnimation.updateAnimation(gt);
                    break;
                case AnimationState.Hurt:
                    break;
            }

            if (health < 0) {
                Game1.isGameOver = true;
            }
        }

        public void Draw(SpriteBatch sb) {
            switch (currentAnimation) {
                case AnimationState.Idle:
                    idleAnimation.drawAnimation(sb, new Vector2((int)Game1.camera.WorldToScreen(position * 32).X + 1, (int)Game1.camera.WorldToScreen(position * 32).Y + 1));
                    break;
                case AnimationState.Hurt:
                    break;
            }
        }

        public static void TakeDamage(int damage) {
            health -= damage;
        }
    }
}