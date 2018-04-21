using LudumDare41_Game.CoordinateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDare41_Game.Towers {
    abstract class Tower {

        public abstract TileCoord Coord { get; }
        public abstract TowerSize Size { get; }

        public abstract TowerDmgPotential DamagePotential { get; }
        public abstract TowerMaxHealth MaxHealth { get; }
        public abstract int Tier { get; }
        public abstract WeaponType WpnType { get; }
        public abstract AnimationState AnimState { get; }

        public abstract void Init (TileCoord _coord);
        public abstract void Update (GameTime _gameTime);
        public abstract void Draw (SpriteBatch spriteBatch);

        public abstract void Damage (int amount);
        public abstract void Repair (int amount);
    }

    public enum TowerDmgPotential { ExtremlyLow, Low, MediumLow, Medium, MediumHigh, High, ExtremlyHigh }
    public enum TowerMaxHealth { ExtremlyLow, Low, MediumLow, Medium, MediumHigh, High, ExtremlyHigh }
    public enum WeaponType { Ray, Arrow, Bomb }
    public enum AnimationState { Attack, Idle }

}
