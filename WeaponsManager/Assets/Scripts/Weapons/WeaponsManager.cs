using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class WeaponsManager : MonoBehaviour
    {
        public static WeaponsManager instance;
        private Weapon currentWeapon;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }



        public Weapon GetWeapon()
        {
            if (currentWeapon != null)
                return currentWeapon;
            else
                return null;
        }

        public void SetWeapon(Weapon weapon)
        {
            if (currentWeapon != weapon)
            {
                Debug.Log("***WeaponManager Changed weapon to " + weapon.ToString());
                currentWeapon = weapon;
            }
        }

        public void StartFireWeaponAnimation()
        {
            if (!CameraManager.instance.inDefaultPosition)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.Fire();
                }
            }
            else
            {
                Debug.Log("No Weapon has been selected - you are in default view");
            }
        }


        public void LoadProjectile()
        {
            if (!CameraManager.instance.inDefaultPosition)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.LoadProjectile();
                }
            }
            else
            {
                Debug.Log("No Weapon has been selected - you are in default view");
            }

        }

        /**
         * Informs the current weapon that the one of its projectiles needs to be destroyed
         * 
         */
        public void DestroyFiredProjectile(GameObject projectile)
        {
            if (projectile != null)
            {
                currentWeapon.DestroyFiredProjectile(projectile);
                StopWeaponShowingFiringInProgress();
            }
            
        }

        /**
         *Inform the weapon that fired that it is no longer firing
         */
        private void StopWeaponShowingFiringInProgress()
        {
            currentWeapon.NoLongerFiringInProgress();
        }



    }
}
