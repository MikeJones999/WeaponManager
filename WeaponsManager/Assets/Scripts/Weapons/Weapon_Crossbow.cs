using UnityEngine;

public class Weapon_Crossbow : Weapon
{

        public override void Fire()
        {
            Debug.Log("Fired : Crossbow");
        }

        public override string ToString()
        {
            return "Crossbow";
        }

}