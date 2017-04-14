using System;
using UnityEngine;

public class Weapon_Crossbow : Weapon
{

        public override void Fire()
        {
            Debug.Log("Fired : Crossbow");

        }

    public override void FireProjectile()
    {
        throw new NotImplementedException();
    }

    public override void LoadAmmo()
    {
        throw new NotImplementedException();
    }

    public override void SwitchAmmo(GameObject ammo, int ammoCount)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
        {
            return "Crossbow";
        }

}