using System.Collections.Generic;
using LudumDare41_Game.Content;
using LudumDare41_Game.Graphics;
using LudumDare41_Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare41_Game.Entities {
    class TestEntity : Entity {
        private Vector2 position;
        public override Vector2 Position { get { return position; } }
        public override List<Vector2> Path { get; }
        public override float Speed { get; }

        public override EntityHealth Health { get; }
        private EntityAnimationState animationState;
        public override EntityAnimationState AnimationState { get; }

        private EntitySize size;
        public override EntitySize Size { get { return size; } }

        private Animation idle;

        private ContentManager contentManager;
        private CoordHandler coordHandler;

        public TestEntity (ContentManager _contentManager, CoordHandler _coordHandler) {
            contentManager = _contentManager;
            coordHandler = _coordHandler;
        }

        public override void Init (Vector2 _position) {
            position = _position;
            animationState = EntityAnimationState.Idle;
            size = new EntitySize(EntityWidth.narrow, EntityHeight.medium);

            idle = new Animation(contentManager.Load<Texture2D>("Entities/ExampleEntity/ExampleEnemy"), new Vector2((int)size.Width, (int)size.Height), 1, 4f);
        }

        public override void Update (GameTime gameTime) {
            switch (animationState) {
                case EntityAnimationState.Attack:
                    //attackAnimation.updateAnimation(gameTime);
                    break;
                case EntityAnimationState.Idle:
                    idle.updateAnimation(gameTime);
                    break;
                default:
                    break;
            }
        }

        public override void Draw (SpriteBatch spriteBatch) {
            switch (animationState) {
                case EntityAnimationState.Attack:
                    //attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? (towerManager.TowerAtCoord(this.coord) ? new Color(Color.Red, 0.5f) : new Color(Color.White, 0.5f)) : Color.White);
                    break;
                case EntityAnimationState.Idle:
                    idle.drawAnimation(spriteBatch, coordHandler.WorldToScreen(position), Color.White);
                    break;
                default:
                    break;
            }
        }
    }
}
