﻿using System;
using System.Collections.Generic;
using LudumDare41_Game.Content;
using LudumDare41_Game.Graphics;
using LudumDare41_Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare41_Game.Entities {
    class EnemyEntity : Entity {
        private Vector2 position;
        public override Vector2 Position { get { return position; } }
        public override List<PathPoint> Path { get => path; }
        private List<PathPoint> path;
        public override float Speed { get => speed; }
        private float speed;

        private int currentHealth;
        public override int CurrentHealth { get => currentHealth; }
        private EntityHealth initHealth;
        public override EntityHealth Health { get { return initHealth; } }
        private EntityAnimationState animationState;
        public override EntityAnimationState AnimationState { get; }

        private EntitySize size;
        public override EntitySize Size { get { return size; } }

        private EntityAttackPower attackPower = EntityAttackPower.medium;
        private EntityAttackSpeed attackSpeed = EntityAttackSpeed.medium;

        public override EntityAttackPower AttackPower { get => attackPower; }
        public override EntityAttackSpeed AttackSpeed { get => attackSpeed; }

        private Animation idle;

        private ContentManager contentManager;
        private CoordHandler coordHandler;
        private EntityManager entityManager;

        public EnemyEntity (EntityManager _entityManager) {
            entityManager = _entityManager;
            contentManager = entityManager.ContentManager;
            coordHandler = entityManager.CoordHandler;
        }

        public override void Init_Wave (Vector2 _position) {
            position = _position;
            animationState = EntityAnimationState.Idle;
            size = new EntitySize(EntityWidth.narrow, EntityHeight.medium);

            idle = new Animation(contentManager.Load<Texture2D>("Entities/ExampleEntity/ExampleEnemy"), new Vector2((int)size.Width, (int)size.Height), 1, 4f);

            path = new List<PathPoint>();

            foreach (var point in entityManager.WaveManager.Path) {
                path.Add(new PathPoint(point.Position + new Vector2(((float)entityManager.Random.Next(15, 100)) / 100, ((float)entityManager.Random.Next(15, 100)) / 100)));
            }

            for (int i = 0; i < path.Count; i++)
                path[i].SetScore(i);

            speed = 50f;

            initHealth = EntityHealth.medium;
            currentHealth = (int)initHealth;
        }

        public override void Update (GameTime gameTime) {
            switch (animationState) {
                case EntityAnimationState.Attack:
                    //attackAnimation.updateAnimation(gameTime);
                    break;
                case EntityAnimationState.Idle:
                    idle.updateAnimation(gameTime);
                    if (path.Count > 0 && MoveTowardsPoint(path[0].Position, gameTime))
                        path.RemoveAt(0);
                    break;
                default:
                    break;
            }
        }

        public override void Draw (SpriteBatch spriteBatch) {
            switch (animationState) {
                case EntityAnimationState.Attack:
                    //attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? (towerManager.TowerAtCoord(this.coord) ? new Color(Color.Red, 0.5f) : new Color(Color.White, 0.5f)) : Color.White);
                    break;
                case EntityAnimationState.Idle:
                    idle.drawAnimation(spriteBatch, coordHandler.WorldToScreen(position), Color.White);
                    break;
                default:
                    break;
            }
        }

        private bool MoveTowardsPoint (Vector2 goal, GameTime gameTime) {
            // If we're already at the goal return immediatly
            if (position == goal) return true;

            // Find direction from current position to goal
            Vector2 direction = Vector2.Normalize(goal - position);

            // Move in that direction
            position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // If we moved PAST the goal, move it back to the goal
            if (Math.Abs(Vector2.Dot(direction, Vector2.Normalize(goal - position)) + 1) < 0.1f)
                position = goal;

            // Return whether we've reached the goal or not
            return position == goal;
        }

        public override void TakeDamage (int amount) {
            currentHealth -= amount;

            if (currentHealth <= 0)
                entityManager.Kill(this);
        }
    }
}
