using System;
using UnityEngine;

public class Weapon_Crossbow : Weapon
{

     
    public override void FireProjectile()
    {
        throw new NotImplementedException();
    }

    public override void LoadProjectile()
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