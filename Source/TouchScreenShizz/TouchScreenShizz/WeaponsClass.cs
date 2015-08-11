using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace Weapons
{

    public enum WeaponType
    {
        Handgun,
        Rifle,
        MachineGun,
        Flamethrower,
        Grenade,
        RPG
    }

    public class Weapon
    {
        //ammo
        private int ammo;
        //ammo till next reload
        private int ammoTillReload;
        //reload delay
        private double reloadDelay;
        //delay between shots
        private double shotDelay;
        //kills to unlock
        private int killsToUnlock;
        //damage
        private int damage;

        public Weapon(WeaponType weaponType)
        {
            if (weaponType == WeaponType.Handgun)
            {
                ammo = 12;
                ammoTillReload = 6;
                reloadDelay = 0.5;
                shotDelay = 0.01;
                killsToUnlock = 0;
                damage = 50;

            }

            if (weaponType == WeaponType.Rifle)
            {
                ammo = 60;
                ammoTillReload = 20;
                reloadDelay = 1.5;
                shotDelay = 0.01;
                killsToUnlock = 50;
                damage = 2;

            }

            if (weaponType == WeaponType.MachineGun)
            {
                ammo = 200;
                ammoTillReload = 40;
                reloadDelay = 2;
                shotDelay = 0.001;
                killsToUnlock = 120;
                damage = 1;

            }

            if (weaponType == WeaponType.Flamethrower)
            {
                ammo = 0;
                ammoTillReload = 0;
                reloadDelay = 0;
                shotDelay = 0;
                killsToUnlock = 200;
                damage = 1;

            }

            if (weaponType == WeaponType.Grenade)
            {
                ammo = 3;
                ammoTillReload = 1;
                reloadDelay = 0.5;
                shotDelay = 0.5;
                killsToUnlock = 70;
                damage = 9001;

            }

            if (weaponType == WeaponType.RPG)
            {
                ammo = 6;
                ammoTillReload = 1;
                reloadDelay = 1;
                shotDelay = 1;
                killsToUnlock = 300;
                damage = 9001;

            }
        }

        public int Damage(int health)
        {
            health = health - damage;
            return health;
        }
    }

    public class HotWeapons : Weapon
    {
        private int heat;
        private int maxHeat;
        private double cooldownTime;
        private int heatPerSec;

        public HotWeapons(WeaponType weaponType)
            : base(weaponType)
        {
            if (weaponType == WeaponType.MachineGun)
            {
                heat = 0;
                maxHeat = 100;
                heatPerSec = 10;
                cooldownTime = 2;

            }

            if (weaponType == WeaponType.Flamethrower)
            {
                heat = 0;
                maxHeat = 100;
                heatPerSec = 15;
                cooldownTime = 2.5;

            }
        }
    }

    public class ExplosiveWeapons : Weapon
    {
        private int explosiveRadius;

        public ExplosiveWeapons(WeaponType weaponType)
            : base(weaponType)
        {
            if (weaponType == WeaponType.Grenade)
            {
                explosiveRadius = 50;

            }

            if (weaponType == WeaponType.RPG)
            {
                explosiveRadius = 100;

            }
        }
    }
}