using Microsoft.Xna.Framework;

namespace LudumDare41_Game.Entities {
    struct PathPoint {
        private Vector2 position;
        public Vector2 Position { get { return position; } }
        private int score;
        public int Score { get { return score; } }

        public PathPoint (Vector2 _position) {
            position = _position;
            score = 0;
        }

        public PathPoint (float x, float y) {
            position = new Vector2(x, y);
            score = 0;
        }

        public void SetScore (int num) {
            score = num;
        }
    }
}
