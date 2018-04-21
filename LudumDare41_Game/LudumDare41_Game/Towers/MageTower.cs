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

        public override bool IsPreviewTower { get; }
        #endregion

        private Animation idleAnimation;
        private Animation attackAnimation;

        private TowerManager towerManager;
        private ContentManager contentManager;

        public MageTower (TowerManager _towerManager, ContentManager _contentManager, bool _isPreviewTower = false) {
            towerManager = _towerManager;
            contentManager = _contentManager;
            IsPreviewTower = _isPreviewTower;
        }

        public override void Init (TileCoord _coord) {

            size = new TowerSize(TowerWidth.narrow, TowerHeight.tall);

            Texture2D temp = contentManager.Load<Texture2D>("Towers/MageTower/MageTower_IdleSpritesheet");
            idleAnimation = new Animation(temp, new Vector2((int)size.Width, (int)size.Height), 2, 350f);
            //attack animation

            coord = _coord;
            animState = AnimationState.Idle;
        }

        public override void Update (GameTime gameTime) {
            switch (animState) {
                case AnimationState.Attack:
                    //attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord));
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
                    //attackAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord));
                    break;
                case AnimationState.Idle:
                    idleAnimation.drawAnimation(spriteBatch, towerManager.GetDrawPos(coord), IsPreviewTower ? new Color(Color.White, 0.5f) : Color.White);
                    break;
                default:
                    break;
            }
        }

        public override void Damage (int amount) {

        }

        public override void Repair (int amount) {

        }

        public override void MoveTo (TileCoord _coord) {
            coord = _coord;
        }
    }
}
