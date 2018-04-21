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

        public static bool operator == (TileCoord coord_1, TileCoord coord_2) {
            return (coord_1.x == coord_2.x) && (coord_1.y == coord_2.y);
        }

        public static bool operator != (TileCoord coord_1, TileCoord coord_2) {
            return !(coord_1.x == coord_2.x) && (coord_1.y == coord_2.y);
        }
    }
}
