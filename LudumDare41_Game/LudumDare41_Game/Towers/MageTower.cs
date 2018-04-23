using System.Collections.Generic;
using LudumDare41_Game.Content;
using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Entities;
using LudumDare41_Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare41_Game.Towers {
    class MageTower : Tower {

        #region Tower properties
        private TileCoord coord;
        public override TileCoord Coord { get { return coord; } }
        private TowerSize size;
        public override TowerSize Size { get; }

        private int dmgPotential;
        public override TowerDmgPotential DamagePotential { get; }
        private float timeSinceLastAttack = 0;
        private TowerAttackCooldown attackCooldown;
        public override TowerAttackCooldown AttackCooldown { get { return attackCooldown; } }
        public override TowerMaxHealth MaxHealth { get; }
        private int currentHealth;
        public override int Tier { get; }
        public override WeaponType WpnType { get; }
        private float attackRadiusSquared;
        private TowerAttackRadius attackRadius;
        public override TowerAttackRadius AttackRadius { get { return attackRadius; } }
        private List<Entity> sightedEntities;
        public override List<Entity> SightedEntities { get; }
        private TowerAnimationState animState;
        public override TowerAnimationState AnimState { get { return animState; } }

        public override bool IsPreviewTower { get; }

        #endregion

        private Animation idleAnimation;
        private Animation attackAnimation;

        private TowerManager towerManager;
        private ContentManager contentManager;
        private EntityManager entityManager;

        //private Entity nearestEntity;
        private Entity targetEntity = null;

        private Texture2D circle;

        public MageTower (TowerManager _towerManager, ContentManager _contentManager, EntityManager _entityManager, bool _isPreviewTower = false) {
            towerManager = _towerManager;
            contentManager = _contentManager;
            entityManager = _entityManager;
            IsPreviewTower = _isPreviewTower;
        }

        public override void Init (TileCoord _coord) {

            size = new TowerSize(TowerWidth.narrow, TowerHeight.tall);

            idleAnimation = contentManager.LoadAnimation("MageTower", "Idle", (int)size.Width, (int)size.Height, 350f);
            attackAnimation = contentManager.LoadAnimation("MageTower", "Attack", (int)size.Width, (int)size.Height, 350f);
            //attack animation

            circle = contentManager.Load<Texture2D>("Towers/Debug/Circle");

            coord = _coord;
            animState = TowerAnimationState.Idle;

            //nearestEntity = entityManager.Dummy;
            attackRadius = TowerAttackRadius.medium;
            attackRadiusSquared = (float)System.Math.Pow((double)attackRadius, 2);
            dmgPotential = (int)TowerDmgPotential.Medium;
            attackCooldown = TowerAttackCooldown.medium;
            sightedEntities = new List<Entity>();
        }

        public override void Update (GameTime gameTime) {

            if (!IsPreviewTower) {

                sightedEntities.Clear();

                for (int i = 0; i < entityManager.Entities.Count; i++) {
                    if ((entityManager.Entities[i].Position - this.coord.ToVector2()).LengthSquared() < attackRadiusSquared) {

                        sightedEntities.Add(entityManager.Entities[i]);

                        //if ((entityManager.Entities[i].Position - this.coord.ToVector2()).LengthSquared() < (nearestEntity.Position - this.coord.ToVector2()).LengthSquared()) {
                        //    nearestEntity = entityManager.Entities[i];
                        //    animState = TowerAnimationState.Attack;
                        //    targetEntity = entityManager.Entities[i];
                        //}
                    }
                }
            }

            if (sightedEntities.Count != 0) {
                int min = 1000;
                int index = 0;

                for (int i = 0; i < sightedEntities.Count; i++)
                    if (sightedEntities[i].CurrentHealth > min) {
                        min = sightedEntities[i].CurrentHealth;
                        index = i;
                    }


                targetEntity = sightedEntities[index];
            }
            else {
                targetEntity = null;
            }

            if (targetEntity != null) {
                animState = TowerAnimationState.Attack;
            }
            else if (targetEntity == null && sightedEntities.Count != 0) {

            }
            else {
                animState = TowerAnimationState.Idle;
            }

            switch (animState) {
                case TowerAnimationState.Attack:
                    attackAnimation.updateAnimation(gameTime);
                    if (timeSinceLastAttack > (int)attackCooldown / 2) {
                        targetEntity.TakeDamage(dmgPotential);
                        timeSinceLastAttack = 0;
                    }
                    break;
                case TowerAnimationState.Idle:
                    idleAnimation.updateAnimation(gameTime);
                    break;
                default:
                    break;
            }

            timeSinceLastAttack += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw (SpriteBatch spriteBatch) {
            switch (animState) {
                case TowerAnimationState.Attack:
                    attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? (towerManager.InvalidCoord(this.coord) ? new Color(Color.Red, 0.5f) : new Color(Color.White, 0.5f)) : Color.White);
                    break;
                case TowerAnimationState.Idle:
                    idleAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? (towerManager.InvalidCoord(this.coord) ? Color.Red : Color.White) : Color.White);
                    break;
                default:
                    break;
            }

            if (IsPreviewTower) {
                //int width = 4 * (int)attackRadius + 64, height = 4 * (int)attackRadius + 64;
                //spriteBatch.Draw(circle, new Rectangle((int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).X - width / 2 + 32, (int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).Y - height / 2, width, height), Color.Gray * 0.3f);
                MonoGame.Extended.ShapeExtensions.DrawCircle(spriteBatch, new MonoGame.Extended.CircleF(new Point((int)entityManager.CoordHandler.WorldToScreen(coord.ToVector2()).X + 32, (int)entityManager.CoordHandler.WorldToScreen(coord.ToVector2()).Y + 32), (float)attackRadius * 2), 30, Color.White, 10);
            }

            if (towerManager.Game.DebugMode) {
                if (!this.IsPreviewTower) {
                    //int width = 4 * (int)attackRadius + 64, height = 4 * (int)attackRadius + 64;
                    //spriteBatch.Draw(circle, new Rectangle((int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).X - width / 2 + 32, (int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).Y - height / 2 + 32, width, height), Color.Green * 0.3f);
                    MonoGame.Extended.ShapeExtensions.DrawCircle(spriteBatch, new MonoGame.Extended.CircleF(new Point ((int)entityManager.CoordHandler.WorldToScreen(coord.ToVector2()).X + 32, (int)entityManager.CoordHandler.WorldToScreen(coord.ToVector2()).Y + 32), (float)attackRadius * 2), 30, Color.Green * 0.4f, 10);
                }
            }
        }

        public override void TakeDamage (int amount) {
            currentHealth -= amount;
        }

        public override void RepairDamage (int amount) {
            currentHealth += amount;
        }

        public override void MoveTo (TileCoord _coord) {
            coord = _coord;
        }
    }
}
