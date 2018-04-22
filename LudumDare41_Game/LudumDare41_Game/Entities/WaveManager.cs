using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    class WaveManager {
        private List<PathPoint> path = new List<PathPoint> {
                new PathPoint(320, 100),
                new PathPoint(570, 300),
                new PathPoint(600, 300),
                new PathPoint(780, 480),
                new PathPoint(780, 545),
                new PathPoint(720, 580),
                new PathPoint(630, 580),
                new PathPoint(440, 470),
                new PathPoint(210, 470),
                new PathPoint(210, 550),
                new PathPoint(470, 780),
                new PathPoint(575, 780),
                new PathPoint(686, 930)
            };

        public List<PathPoint> Path { get { return path; } }
    }

    class Wave {
        private int totalAmntOfEnemies;
        public int TotalAmntOfEnemies { get { return totalAmntOfEnemies; } }
        private List<Entity> enemies;
        public List<Entity> Enemies { get { return enemies; } }

        private EntityManager entityManager;

        public Wave (List<Entity> _enemies, EntityManager _entityManager) {
            enemies = _enemies;
            totalAmntOfEnemies = enemies.Count;
            entityManager = _entityManager;
        }

        //public void SpawnEnemy () {
        //    entityManager.SpawnEntity();
        //}
    }
}
