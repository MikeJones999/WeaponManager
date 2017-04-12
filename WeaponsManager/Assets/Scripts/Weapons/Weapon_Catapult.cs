using System;
using UnityEngine;


namespace Assets.Scripts.Weapons
{
    public class Weapon_Catapult : Weapon
    {
        public GameObject Catapult;
        public GameObject Ammo;
        public GameObject AmmoLoadPos;
        private GameObject AmmoHolder;
        public float ForceApplied;

        

        public override void Fire()
        {
            if (weaponLoaded)
            {
                Debug.Log("Fired : Catapult");
                Catapult.GetComponent<Animator>().Play("FireShot");
                AmmoCount--;

            }
            else
            {
                Debug.Log("No ammo loaded - handle with UI notice!!!!");
            }
        }


        public override void FireAmmo()
        {




            //disconnect ammo from parent
            var temp = AmmoHolder.transform.parent;
            AmmoHolder.transform.parent = null;

            AmmoHolder.GetComponent<Collider>().enabled = true;


            //initialise rigidbody's gravity
            AmmoHolder.GetComponent<Rigidbody>().useGravity = true;
            //Get angle, if not 45deg set to 45deg


            AmmoHolder.transform.Rotate (-45,90,0);

            //addforce (fire) using ammo's force parameter
            AmmoHolder.GetComponent<Rigidbody>().AddForce(AmmoHolder.transform.forward * ForceApplied, ForceMode.Acceleration);

            //Weapon shown as not loaded
            weaponLoaded = false;


        }






        public override void LoadAmmo()
        {
            //test purposes
            SwitchAmmo(Ammo, 5);


            if (!weaponLoaded)
            {
                if (Ammo != null && AmmoCount > 0)
                {  
                    if (AmmoLoadPos != null)
                    {
                        AmmoHolder = Instantiate(Ammo, new Vector3(AmmoLoadPos.transform.position.x, AmmoLoadPos.transform.position.y + 0.15f, AmmoLoadPos.transform.position.z), Quaternion.identity) as GameObject;

                        if (AmmoHolder != null)
                        {
                            AmmoHolder.transform.parent = AmmoLoadPos.transform;
                        }

                        //Instantiate(Ammo, new Vector3(loadPos.position.x, loadPos.position.y + 0.15f, loadPos.position.z), Quaternion.identity);
                        weaponLoaded = true;
                    }
                    else
                    {
                        //Error Cant find loading pos
                        Debug.Log("Error Cant find loading pos for Ammo - or you have not identified it in the script");
                    }
                        
                    
                }
                else
                {
                    weaponLoaded = false;
                    Debug.Log("No ammo loaded or out of ammo - handle with UI notice!!!!");
                }
            }
            else
            {
                Debug.Log("Weapon already loaded");
            }

        }

        

        public override void SwitchAmmo(GameObject ammo, int ammoCount)
        {
            this.Ammo = ammo;
            this.AmmoCount = ammoCount;
        }

        public override string ToString()
        {
            return "Catapult";
        }
    }
}
