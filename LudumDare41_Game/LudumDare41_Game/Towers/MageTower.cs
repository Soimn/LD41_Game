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
        public override TowerMaxHealth MaxHealth { get; }
        private int currentHealth;
        public override int Tier { get; }
        public override WeaponType WpnType { get; }
        private float attackRadiusSquared;
        private TowerAttackRadius attackRadius;
        public override TowerAttackRadius AttackRadius { get { return attackRadius; } }
        private TowerAnimationState animState;
        public override TowerAnimationState AnimState { get { return animState; } }

        public override bool IsPreviewTower { get; }
        #endregion

        private Animation idleAnimation;
        private Animation attackAnimation;

        private TowerManager towerManager;
        private ContentManager contentManager;
        private EntityManager entityManager;

        private Entity nearestEntity;
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

            nearestEntity = entityManager.Dummy;
            attackRadius = TowerAttackRadius.medium;
            attackRadiusSquared = (float)System.Math.Pow((double)attackRadius, 2);
            dmgPotential = (int)TowerDmgPotential.Medium;
        }

        public override void Update (GameTime gameTime) {

            for (int i = 0; i < entityManager.Entities.Count; i++) {
                if ((entityManager.Entities[i].Position - this.coord.ToVector2()).LengthSquared() < attackRadiusSquared) {
                    if ((entityManager.Entities[i].Position - this.coord.ToVector2()).LengthSquared() < (nearestEntity.Position - this.coord.ToVector2()).LengthSquared()) {
                        nearestEntity = entityManager.Entities[i];
                        animState = TowerAnimationState.Attack;
                        targetEntity = entityManager.Entities[i];
                    }
                }
            }

            if (((nearestEntity.Position - this.coord.ToVector2()).LengthSquared() > attackRadiusSquared) || targetEntity == null) {
                nearestEntity = entityManager.Dummy;
                animState = TowerAnimationState.Idle;
                targetEntity = null;
            }

            switch (animState) {
                case TowerAnimationState.Attack:
                    attackAnimation.updateAnimation(gameTime);
                    targetEntity.TakeDamage(dmgPotential);
                    break;
                case TowerAnimationState.Idle:
                    idleAnimation.updateAnimation(gameTime);
                    break;
                default:
                    break;
            }
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
                int width = 4 * (int)attackRadius + 64, height = 4 * (int)attackRadius + 64;
                spriteBatch.Draw(circle, new Rectangle((int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).X - width / 2 + 32, (int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).Y - height / 2 + 32, width, height), Color.Gray * 0.3f);
            }

            if (towerManager.Game.DebugMode) {
                int width = 4 * (int)attackRadius + 64, height = 4 * (int)attackRadius + 64;
                spriteBatch.Draw(circle, new Rectangle((int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).X - width / 2 + 32, (int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).Y - height / 2 + 32, width, height), Color.Green * 0.3f);
            }

            System.Console.WriteLine(towerManager.coordHandler.WorldToScreen(coord.ToVector2()));
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
