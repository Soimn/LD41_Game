﻿using LudumDare41_Game.CoordinateSystem;
using LudumDare41_Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace LudumDare41_Game.Towers {
    abstract class Tower {

        public static Dictionary<string, Type> towerID = new Dictionary<string, Type>() { { "MageTower", typeof(MageTower) }, { "BombTower", typeof(BombTower) } };

        public abstract TileCoord Coord { get; }
        public abstract TowerSize Size { get; }

        public abstract bool IsPreviewTower { get; }

        public abstract TowerDmgPotential DamagePotential { get; }
        public abstract TowerAttackCooldown AttackCooldown { get; }
        public abstract TowerMaxHealth MaxHealth { get; }
        public abstract int Tier { get; }
        public abstract WeaponType WpnType { get; }
        public abstract TowerAttackRadius AttackRadius { get; }
        public abstract List<Entity> SightedEntities { get; }
        public abstract TowerAnimationState AnimState { get; }

        public abstract void Init (TileCoord _coord);
        public abstract void Update (GameTime _gameTime);
        public abstract void Draw (SpriteBatch spriteBatch);

        public abstract void MoveTo (TileCoord _coord);

        public abstract void TakeDamage (int amount);
        public abstract void RepairDamage (int amount);
    }

    public enum TowerDmgPotential { ExtremlyLow = 1, Low = 2, Medium = 3, High = 5, ExtremlyHigh = 10 }
    public enum TowerAttackCooldown { fast = 1, medium = 2, slow = 4 }
    public enum TowerMaxHealth { ExtremlyLow, Low, MediumLow, Medium, MediumHigh, High, ExtremlyHigh }
    public enum WeaponType { Ray, Arrow, Bomb }
    public enum TowerAttackRadius { small = 2 * 32, medium = 4 * 32, large = 8 * 32 }
    public enum TowerAnimationState { Attack, Idle }
}
