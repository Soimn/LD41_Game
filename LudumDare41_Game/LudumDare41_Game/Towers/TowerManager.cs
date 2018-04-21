using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Physics;

namespace LudumDare41_Game.Towers {
    class TowerManager {

        private CoordHandler coordHandler;

        public TowerManager (CoordHandler _coordHandler) {
            coordHandler = _coordHandler;
        }

        public void SpawnTower (Tower tower, TileCoord coord) {
            tower.Init(coord);
        }
    }
}
