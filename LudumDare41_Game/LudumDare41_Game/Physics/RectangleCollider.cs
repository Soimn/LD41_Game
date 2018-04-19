using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LudumDare41_Game.Physics {
    struct RectangleCollider {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Rotation { get; private set; }

        public List<Vector2> Vertecies { get; private set; }

        public Vector2 Position { get; private set; }

        public RectangleCollider (Vector2 _position, int _width, int _height, float _rot = 0) {
            Width = _width;
            Height = _height;
            Position = _position;
            Rotation = _rot;
            Vertecies = CollisionManager.GetRectangleCollisionVertecies(Width, Height);
        }

        public void Rotate (float angle) {
            Rotation += angle;
        }
    }
}