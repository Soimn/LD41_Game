using LudumDare41_Game.Content;
using LudumDare41_Game.CoordinateSystem;
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
        public override int Tier { get; }
        public override WeaponType WpnType { get; }
        private AnimationState animState;
        public override AnimationState AnimState { get { return animState; } }
        #endregion

        private Animation idleAnimation;
        private Animation attackAnimation;

        private TowerManager towerManager;
        private ContentManager contentManager;

        public MageTower (TowerManager _towerManager, ContentManager _contentManager) {
            towerManager = _towerManager;
            contentManager = _contentManager;
        }

        public override void Init (TileCoord _coord) {
            Texture2D temp = contentManager.Load<Texture2D>("MageTower_IdleSpritesheet");
            idleAnimation = new Animation(temp, new Vector2((float)size.Width, (float)size.Height), temp.Width / 32, 3.5f);
            temp = contentManager.Load<Texture2D>("MageTower_AttackSpritesheet");
            attackAnimation = new Animation(temp, new Vector2((float)size.Width, (float)size.Height), temp.Width / 32, 3.5f);

            coord = _coord;
        }

        public override void Update (GameTime gameTime) {

        }

        public override void Draw (SpriteBatch spriteBatch) {
            switch (animState) {
                case AnimationState.Attack:
                    attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord));
                    break;
                case AnimationState.Idle:
                    idleAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord));
                    break;
                default:
                    break;
            }
        }

        public override void Damage (int amount) {

        }

        public override void Repair (int amount) {

        }
    }
}
