using Microsoft.Xna.Framework;

struct CircleCollider {
    public Vector2 Position { get; private set; }
    public float Radius { get; private set; }

    public CircleCollider (Vector2 _position, float _radius) {
        Position = _position;
        Radius = _radius;
    }
}