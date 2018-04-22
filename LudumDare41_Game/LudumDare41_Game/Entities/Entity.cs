using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    abstract class Entity {
        public abstract Vector2 Position { get; }
        public abstract List<Vector2> Path { get; }
        public abstract float Speed { get; }
        public abstract EntitySize Size { get; }

        public abstract EntityHealth Health { get; }
        public abstract EntityAnimationState AnimationState { get; }

        public abstract void Init (Vector2 _position);
        public abstract void Update (GameTime gameTime);
        public abstract void Draw (SpriteBatch spriteBatch);

        public static Vector2 GetDirection (Vector2 position, Vector2 point) {
            Vector2 dir = (point - position);
            dir.Normalize();
            return dir;
        }
    }

    public enum EntityHealth { xxsl = 5, low = 10, medium = 20, large = 80, xxl = 200, xxxl = 2000 }
    public enum EntityAnimationState { Idle, Attack }
}
