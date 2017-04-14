using Assets.Scripts.Projectiles;
using System;
using UnityEngine;


namespace Assets.Scripts.Weapons
{
    public class Weapon_Catapult : Weapon
    {
        public GameObject Catapult;
        public GameObject Ammo;
        public GameObject AmmoLoadPos;
        

        

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


        public override void FireProjectile()
        {
            //disconnect ammo from parent
           // var temp = AmmoProjectile.transform.parent;
            AmmoProjectile.transform.parent = null;

            //enable the collider on the ammo so it can connect with other objects
            AmmoProjectile.GetComponent<Collider>().enabled = true;


            //initialise rigidbody's gravity
            AmmoProjectile.GetComponent<Rigidbody>().useGravity = true;
            //Get angle, if not 45deg set to 45deg


            AmmoProjectile.transform.Rotate (-45,90,0);

            //addforce (fire) using ammo's force parameter
            AmmoProjectile.GetComponent<Rigidbody>().AddForce(AmmoProjectile.transform.forward * ProjectileForceApplied, ForceMode.Acceleration);

            //Weapon shown as not loaded
            weaponLoaded = false;


            //camera follow until object destroyed
            CameraFollowProjectile();
        }



        public override void LoadAmmo()
        {

            

            //test purposes
            SwitchAmmo(Ammo, 5);

            //if weapon currently doesn't have any projectiles attached then continue to load one
            if (!weaponLoaded)
            {
                if (Ammo != null && AmmoCount > 0)
                {  
                    if (AmmoLoadPos != null)
                    {
                       //There is a a position attached to the weapon that an object can be created at - therefore create a copy of the ammo prefab (attached to this script) and place it at AmmoLoadPos
                        AmmoProjectile = Instantiate(Ammo, new Vector3(AmmoLoadPos.transform.position.x, AmmoLoadPos.transform.position.y + 0.15f, AmmoLoadPos.transform.position.z), Quaternion.identity) as GameObject;
                        
                        //Now that the object is created - get the script attached to it called projectile - and then get the force of this particular projectile type. - Projectile is an abstract class. This saves us having to hard code the value each time for different projectiles
                        this.ProjectileForceApplied = AmmoProjectile.GetComponent<Projectile>().GetProjectileForce();

                        //If the object is actually created then attach it to the AmmoLoadPos - so that it will follow it during any animation
                        if (AmmoProjectile != null)
                        {
                            AmmoProjectile.transform.parent = AmmoLoadPos.transform;
                            //Wepaon is now loaded so stop it being loaded at the moment
                            weaponLoaded = true;
                        }
                        
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
