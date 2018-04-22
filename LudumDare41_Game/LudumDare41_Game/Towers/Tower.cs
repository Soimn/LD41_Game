using LudumDare41_Game.CoordinateSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.Towers {
    abstract class Tower {

        public static Dictionary<string, Type> towerID = new Dictionary<string, Type>() { { "MageTower", typeof(MageTower) } };

        public abstract TileCoord Coord { get; }
        public abstract TowerSize Size { get; }

        public abstract bool IsPreviewTower { get; }

        public abstract TowerDmgPotential DamagePotential { get; }
        public abstract TowerMaxHealth MaxHealth { get; }
        public abstract int Tier { get; }
        public abstract WeaponType WpnType { get; }
        public abstract TowerAttackRadius AttackRadius { get; }
        public abstract AnimationState AnimState { get; }
        
        public abstract void Init (TileCoord _coord);
        public abstract void Update (GameTime _gameTime);
        public abstract void Draw (SpriteBatch spriteBatch);

        public abstract void MoveTo (TileCoord _coord);

        public abstract void Damage (int amount);
        public abstract void Repair (int amount);
    }

    public enum TowerDmgPotential { ExtremlyLow, Low, MediumLow, Medium, MediumHigh, High, ExtremlyHigh }
    public enum TowerMaxHealth { ExtremlyLow, Low, MediumLow, Medium, MediumHigh, High, ExtremlyHigh }
    public enum WeaponType { Ray, Arrow, Bomb }
    public enum TowerAttackRadius { small = 5 * 32, medium = 12 * 32, large = 20 * 32, xxl = 50 * 32, xxxl = 95 * 32}
    public enum AnimationState { Attack, Idle }
}
