using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.Entities {
    class WaveManager {

        public const float TimeBetweenEnemySpawn = 0.5f;

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

        public void Init (EntityManager _entityManager) {
            entityManager = _entityManager;
        }

        public void Update (GameTime gameTime) {
            
        }

        public Round CreateRound (int grade = 1) {

            Round round = new Round();

            switch (grade) {
                case 1:
                    for (int i = 0; i < 10; i++) {
                        round.waves.Add(CreateWave(1, 0, 0, entityManager.Random.Next(5, 20), 0));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return round;
        }

        public Wave CreateWave (int pauseGrade = 1, int numOfHeavyEnemies = 0, int numOfMediumEnemies = 0, int numOfLightEnemies = 1, int numOfBosses = 0) {
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

            if (temp.Count > 16) {

                switch (pauseGrade) {
                    case 1:
                        pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.minor + entityManager.Random.Next(0, 1)));
                        break;
                    case 2:
                        for (int i = 0; i < 2; i++) {
                            pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.low + entityManager.Random.Next(0, 2)));
                        }
                        break;
                    case 3:
                        for (int i = 0; i < 2; i++) {
                            pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.medium + entityManager.Random.Next(0, 3)));
                        }
                        break;
                    case 4:
                        for (int i = 0; i < 2; i++) {
                            pauses.Add(new KeyValuePair<int, float>(entityManager.Random.Next(0 + 8, temp.Count - 8), (float)WavePause.large + entityManager.Random.Next(0, 5)));
                        }
                        break;

                    default:
                        throw new ArgumentException("No such pausegrade as pausegrade: " + pauseGrade);
                }
            }


            return new Wave(temp, entityManager, new WaveInstructor(temp, pauses).CreateInstructions(), numOfBosses != 0 ? 4 : (numOfHeavyEnemies != 0 ? 3 : (numOfMediumEnemies != 0 ? 2 : 1)));
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

        private Stack<WaveInstruction> waveInstructions;
        private WaveInstruction currentInstruction;
        private float timeToWait = 0;
        private float timeSinceLastWait = 0;

        public Wave (List<Type> _enemies, EntityManager _entityManager, Stack<WaveInstruction> _waveInstructions, int _highestGrade) {
            enemies = _enemies;
            totalAmntOfEnemies = enemies.Count;
            entityManager = _entityManager;
            waveInstructions = _waveInstructions;
            highestGrade = _highestGrade;
        }

        public void Update (GameTime gameTime) {

            if (timeSinceLastWait > timeToWait) {
                NextInstruction();
            }

            timeSinceLastWait += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void NextInstruction () {
            currentInstruction = waveInstructions.Pop();

            if (currentInstruction.IsPause) {
                timeToWait = currentInstruction.WaitDuration;
            }
            else {
                Object[] args = { entityManager };

                entityManager.SpawnWaveEntity((Entity)Activator.CreateInstance(currentInstruction.Entity, args));
            }
        }
    }

    struct Round {
        public List<Wave> waves;
    }

    class WaveInstructor {
        public List<Type> entities = new List<Type>();
        public List<float> timeInstructions = new List<float>();

        public WaveInstructor (List<Type> _enemies, List<KeyValuePair<int, float>> pauses) {
            entities = _enemies;

            for (int i = 0; i < entities.Count + pauses.Count; i++) {
                timeInstructions[i] = 1;
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
