using LudumDare41_Game.CoordinateSystem;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LudumDare41_Game.Physics {
    class CoordHandler {

        private Camera2D cam;

        public CoordHandler (Camera2D _cam) {
            cam = _cam;
        }

        public Vector2 WorldToScreen (Vector2 world) {
            return cam.WorldToScreen(world);
        }

        public int ScaleToZoom (int field) {
            return (int)(cam.Zoom * field);
        }
    }
}

