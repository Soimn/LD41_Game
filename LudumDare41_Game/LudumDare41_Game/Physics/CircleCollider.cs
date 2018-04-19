using Microsoft.Xna.Framework;

namespace LudumDare41_Game.Physics {
    struct CircleCollider {
        public Vector2 Position { get; private set; }
        public float Radius { get; private set; }

        public CircleCollider (Vector2 _position, float _radius) {
            Position = _position;
            Radius = _radius;
        }
    }
}