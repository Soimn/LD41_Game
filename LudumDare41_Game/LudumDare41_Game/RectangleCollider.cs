using Microsoft.Xna.Framework;
using System.Collections.Generic;

struct RectangleCollider {
    public float Width { get; private set; }
    public float Height { get; private set; }
    public float Rotation { get; private set; }

    public List<Vector2> Vertecies { get; private set; }

    public Vector2 Position { get; private set; }

    public RectangleCollider (Vector2 _position, float _width, float _height, float _rot) {
        Width = _width;
        Height = _height;
        Position = _position;
        Rotation = _rot;
        Vertecies = CollisionManager.GetRectangleCollisionVertecies(Width, Height);
    }
}