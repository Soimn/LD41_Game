using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    class WaveManager {

        public const float TimeBetweenEnemySpawn = 1;

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

        private EntityManager entityManager;
        public Round Round { get; private set; }

        public void Init (EntityManager _entityManager) {
            entityManager = _entityManager;

            Round = new Round(new List<Wave> { CreateWave(), CreateWave(1, 3, 0, 0, 0), CreateWave(1, 10, 0, 0), CreateWave(1, 15, 0, 0, 0, 0.5f), CreateWave(1, 0, 1, 0, 0, 1), CreateWave(1, 30, 5, 0, 0, 1) }, entityManager);
        }

        public void Update (GameTime gameTime) {
            Round.Update(gameTime);
        }

        public bool IsWaveOngoing () {
            return Round.IsWaveOngoing;
        }

        public int SecondsTillNextWave () {
            return 10 - (int)Round.lastTime;
        }

        public Wave CreateWave (int pauseGrade = 1, int numOfLightEnemies = 0, int numOfMediumEnemies = 0, int numOfHeavyEnemies = 0, int numOfBosses = 0, float timeBetween = 1) {
            List<Type> temp = new List<Type>();

            for (int i = 0; i < numOfLightEnemies; i++) {
                temp.Add((entityManager.GetEntityOfTypeByGrade(1)));
            }

            for (int i = 0; i < numOfMediumEnemies; i++) {
                temp.Add((entityManager.GetEntityOfTypeByGrade(2)));
            }

            for (int i = 0; i < numOfHeavyEnemies; i++) {
                temp.Add((entityManager.GetEntityOfTypeByGrade(3)));
            }

            for (int i = 0; i < numOfBosses; i++) {
                temp.Add((entityManager.GetEntityOfTypeByGrade(4)));
            }

            List<KeyValuePair<int, float>> pauses = new List<KeyValuePair<int, float>>();

            //if (temp.Count > 16) {

            //    switch (pauseGrade) {
            //        case 1:
            //            pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.minor + entityManager.Random.Next(0, 1)));
            //            break;
            //        case 2:
            //            for (int i = 0; i < 2; i++) {
            //                pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.low + entityManager.Random.Next(0, 2)));
            //            }
            //            break;
            //        case 3:
            //            for (int i = 0; i < 2; i++) {
            //                pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.medium + entityManager.Random.Next(0, 3)));
            //            }
            //            break;
            //        case 4:
            //            for (int i = 0; i < 2; i++) {
            //                pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.large + entityManager.Random.Next(0, 5)));
            //            }
            //            break;

            //        default:
            //            throw new ArgumentException("No such pausegrade as pausegrade: " + pauseGrade);
            //    }
            //}

            var count = temp.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = entityManager.Random.Next(i, count);
                var tmp = temp[i];
                temp[i] = temp[r];
                temp[r] = tmp;
            }

            return new Wave(temp, entityManager, new WaveInstructor(temp, pauses).CreateInstructions(), numOfBosses != 0 ? 4 : (numOfHeavyEnemies != 0 ? 3 : (numOfMediumEnemies != 0 ? 2 : 1)), this, timeBetween);
        }
    }

    class Wave {
        private int totalAmntOfEnemies;
        public int TotalAmntOfEnemies { get { return totalAmntOfEnemies; } }
        private List<Type> enemies;
        public List<Type> Enemies { get { return enemies; } }
        private int highestGrade;
        public int HighestGrade { get { return highestGrade; } }

        private EntityManager entityManager;
        private WaveManager waveManager;

        private Stack<WaveInstruction> waveInstructions;
        private WaveInstruction currentInstruction;
        private float timeToWait = 0;
        private float timeSinceLastWait = 0;
        private float timeBetween;
        private Round round;

        private bool isAlive;

        public Wave (List<Type> _enemies, EntityManager _entityManager, Stack<WaveInstruction> _waveInstructions, int _highestGrade, WaveManager _waveManager, float _timeBetween) {
            enemies = _enemies;
            totalAmntOfEnemies = enemies.Count;
            entityManager = _entityManager;
            waveInstructions = _waveInstructions;
            highestGrade = _highestGrade;
            waveManager = _waveManager;
            isAlive = true;
            timeBetween = _timeBetween;
        }

        public void Update (GameTime gameTime) {

            if (timeSinceLastWait > timeToWait) {
                if (waveInstructions.Count != 0)
                    NextInstruction();
                else if (isAlive) {
                    waveManager.Round.WaveInstructionFinished();
                    isAlive = false;
                }

                timeSinceLastWait = 0;
            }

            timeSinceLastWait += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void NextInstruction () {
            currentInstruction = waveInstructions.Pop();

            if (!currentInstruction.IsPause) {
                Object[] args = { entityManager };

                entityManager.SpawnWaveEntity((Entity)Activator.CreateInstance(currentInstruction.Entity, args));
            }

            timeToWait = WaveManager.TimeBetweenEnemySpawn * timeBetween;
        }
    }

    class Round {
        public List<Wave> waves;
        private Wave currentWave;
        private EntityManager entityManager;
        private bool isWaveOngoing = true;
        public bool IsWaveOngoing { get { return isWaveOngoing; } }
        private float timeToWait = 0;
        public float lastTime = 0;
        private bool noEnemiesLeft = false;
        private bool waveInstructionsFinished = false;
        private int enemyCount;


        public Round (List<Wave> _waves, EntityManager _entityManager) {
            waves = _waves;
            entityManager = _entityManager;
        }

        public void NextWave () {
            if (waves.Count != 0)
                currentWave = waves[0];
        }

        public void Update (GameTime gameTime) {
            if (currentWave == null)
                NextWave();

            noEnemiesLeft = entityManager.Entities.Count == 0 ? true : false;
            currentWave.Update(gameTime);

            if (noEnemiesLeft && waveInstructionsFinished) {

                isWaveOngoing = false;

                if (lastTime > timeToWait) {
                    if (waves.Count != 0) {
                        NextWave();
                        waveInstructionsFinished = false;
                        timeToWait = 0;
                        isWaveOngoing = true;
                    }
                }
            }

            if (noEnemiesLeft)
                lastTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void WaveInstructionFinished () {
            waveInstructionsFinished = true;
            lastTime = 0;
            timeToWait = 10;
            waves.RemoveAt(0);
        }
    }

    class WaveInstructor {
        public List<Type> entities = new List<Type>();
        public List<float> timeInstructions = new List<float>();

        public WaveInstructor (List<Type> _enemies, List<KeyValuePair<int, float>> pauses) {
            entities = _enemies;

            for (int i = 0; i < entities.Count + pauses.Count; i++) {
                timeInstructions.Add(1);
            }

            for (int i = 0; i < pauses.Count; i++) {
                timeInstructions[pauses[i].Key] = pauses[i].Value;
            }
        }

        public Stack<WaveInstruction> CreateInstructions () {
            Stack<WaveInstruction> temp = new Stack<WaveInstruction>();

            int offset = 0;

            for (int i = 0; i < timeInstructions.Count; i++) {
                if (timeInstructions[i] == 1) {
                    temp.Push(new WaveInstruction(false, entities[i + offset], WaveManager.TimeBetweenEnemySpawn));
                }
                else if (timeInstructions[i] > 0) {
                    temp.Push(new WaveInstruction(true, null, WaveManager.TimeBetweenEnemySpawn * timeInstructions[i]));
                    offset++;
                }
            }

            return temp;
        }
    }

    struct WaveInstruction {
        public bool IsPause { get; private set; }
        public Type Entity { get; private set; }
        public float WaitDuration { get; private set; }

        public WaveInstruction (bool _isPause, Type _entity, float _waitDuration) {
            IsPause = _isPause;
            Entity = _entity;
            WaitDuration = _waitDuration;
        }
    }

    public enum WavePause { minor = 2, low = 4, medium = 8, large = 12 }
}
