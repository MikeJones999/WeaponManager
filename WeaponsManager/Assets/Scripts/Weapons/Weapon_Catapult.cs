using UnityEngine;


namespace Assets.Scripts.Weapons
{
    public class Weapon_Catapult : Weapon
    {
        public GameObject Catapult;
        public override void Fire()
        {
            Debug.Log("Fired : Catapult");
            Catapult.GetComponent<Animator>().Play("FireShot");

        }

        public override string ToString()
        {
            return "Catapult";
        }
    }
}
