using Microsoft.Xna.Framework;

namespace LudumDare41_Game.Physics {
    abstract class RigidBody {

        public abstract Vector2 Positon { get; protected set; }
        public abstract float Rotation { get; protected set; }

        public abstract float Mass { get; protected set; }
        public abstract Vector2 Velocity { get; protected set; }
    }
}
