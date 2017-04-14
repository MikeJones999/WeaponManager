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


        public void LoadAmmo()
        {
            if (!CameraManager.instance.inDefaultPosition)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.LoadAmmo();
                }
            }
            else
            {
                Debug.Log("No Weapon has been selected - you are in default view");
            }

        }

        public void DestroyFiredProjectile()
        {
            if (currentWeapon != null)
            {
                currentWeapon.DestroyFiredProjectile();
            }
            
        }





    }
}
