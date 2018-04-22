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

        public override TowerDmgPotential DamagePotential { get; }
        public override TowerMaxHealth MaxHealth { get; }
        private int currentHealth;
        public override int Tier { get; }
        public override WeaponType WpnType { get; }
        private float attackRadiusSquared;
        private TowerAttackRadius attackRadius;
        public override TowerAttackRadius AttackRadius { get { return attackRadius; } }
        private AnimationState animState;
        public override AnimationState AnimState { get { return animState; } }

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
            animState = AnimationState.Idle;

            nearestEntity = entityManager.Dummy;
            attackRadius = TowerAttackRadius.medium;
            attackRadiusSquared = (float)System.Math.Pow((double)attackRadius, 2);
        }

        public override void Update (GameTime gameTime) {

            for (int i = 0; i < entityManager.Entities.Count; i++) {
                /*if ((((entityManager.Entities[i].Position.X - this.coord.ToVector2().X) * (entityManager.Entities[i].Position.X - this.coord.ToVector2().X)) + ((entityManager.Entities[i].Position.Y - this.coord.ToVector2().Y) * (entityManager.Entities[i].Position.Y - this.coord.ToVector2().Y)) < (float)attackRadius * (float)attackRadius)) {
                    System.Console.WriteLine("ATTACK");
                }*/

                if ((entityManager.Entities[i].Position - this.coord.ToVector2()).LengthSquared() < attackRadiusSquared) {
                    System.Console.WriteLine("ATTACK");
                }
            }

            switch (animState) {
                case AnimationState.Attack:
                    attackAnimation.updateAnimation(gameTime);
                    break;
                case AnimationState.Idle:
                    idleAnimation.updateAnimation(gameTime);
                    break;
                default:
                    break;
            }
        }

        public override void Draw (SpriteBatch spriteBatch) {
            switch (animState) {
                case AnimationState.Attack:
                    attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? (towerManager.InvalidCoord(this.coord) ? new Color(Color.Red, 0.5f) : new Color(Color.White, 0.5f)) : Color.White);
                    break;
                case AnimationState.Idle:
                    idleAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? (towerManager.InvalidCoord(this.coord) ? new Color(Color.Red, 0.5f) : new Color(Color.White, 0.5f)) : Color.White);
                    break;
                default:
                    break;
            }

            spriteBatch.Draw(circle, new Rectangle((int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).X - (((int)attackRadius / 32) * circle.Width) / 2, (int)towerManager.coordHandler.WorldToScreen(coord.ToVector2()).Y, ((int)attackRadius / 32) * circle.Width, ((int)attackRadius / 32) * circle.Height), Color.White);

            System.Console.WriteLine(towerManager.coordHandler.WorldToScreen(coord.ToVector2()));
        }

        public override void Damage (int amount) {
            currentHealth -= amount;
        }

        public override void Repair (int amount) {
            currentHealth += amount;
        }

        public override void MoveTo (TileCoord _coord) {
            coord = _coord;
        }
    }
}
