using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    abstract class Entity {
        public abstract Vector2 Position { get; }
        public abstract List<PathPoint> Path { get; }
        public abstract float Speed { get; }
        public abstract EntitySize Size { get; }

        public abstract int CurrentHealth { get; }
        public abstract EntityHealth Health { get; }
        public abstract EntityAnimationState AnimationState { get; }

        public abstract void Init_Wave (Vector2 _position);
        public abstract void Update (GameTime gameTime);
        public abstract void Draw (SpriteBatch spriteBatch);

        public abstract void TakeDamage (int amount);

        public static Vector2 GetDirection (Vector2 position, Vector2 point) {
            Vector2 dir = (point - position);
            dir.Normalize();
            return dir;
        }
    }

    public enum EntityHealth { low = 6, medium = 12, large = 16 }
    public enum EntityAnimationState { Idle, Attack }
}
