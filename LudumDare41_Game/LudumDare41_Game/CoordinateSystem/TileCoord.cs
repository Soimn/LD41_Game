using Microsoft.Xna.Framework;

namespace LudumDare41_Game.CoordinateSystem {
    struct TileCoord {
        public int x;
        public int y;

        public TileCoord (int _x, int _y) {
            x = _x;
            y = _y;
        }

        public Vector2 ToVector2 () {
            return new Vector2(x, y);
        }

        public static TileCoord Zero () {
            return new TileCoord(0, 0);
        }
    }
}
