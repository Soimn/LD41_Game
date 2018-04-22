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
        public int health { get; set; }
        public static Vector2 position { get; private set; }

        enum AnimationState { Idle, Hurt };
        AnimationState currentAnimation { get; set; }

        private ContentManager content;
        private Animation idleAnimation, hurtAnimation;

        public Home(Vector2 _position, ContentManager _content) {
            content = _content;
            position = _position;

            currentAnimation = AnimationState.Idle;
            idleAnimation = new Animation(_content.Load<Texture2D>("Entities/Home/idleAnim"), new Vector2(64, 64), 3, 750);
            //idleAnimation = new Animation(_content.Load<Texture2D>("Entities/Home/hurtAnim"), new Vector2(64, 64), 3, 750);
        }

        public void Update(GameTime gt) {
            switch (currentAnimation) {
                case AnimationState.Idle:
                    idleAnimation.updateAnimation(gt);
                    break;
                case AnimationState.Hurt:
                    break;
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
    }
}
/*
        public override void Init(Vector2 _position) {
            animationState = EntityAnimationState.Idle;

            size = new EntitySize(EntityWidth.medium, EntityHeight.tall);

        }

        public override void Update(GameTime gt) {
            switch (animationState) {
                case EntityAnimationState.Idle: //default
                    idleAnim.updateAnimation(gt);
                    break;
            }
        }

        public override void Draw(SpriteBatch sb) {
            switch (animationState) {
                case EntityAnimationState.Idle: //default
                    idleAnim.drawAnimation(sb, GetDrawPos(new TileCoord((int)Position.X * 32, (int)Position.Y * 32)));
                    break;
            }
        }

        public Vector2 GetDrawPos(TileCoord coord) {
            return new Vector2((int)coordHandler.WorldToScreen(coord.ToVector2()).X, (int)coordHandler.WorldToScreen(coord.ToVector2()).Y);
        }
    }
}
*/