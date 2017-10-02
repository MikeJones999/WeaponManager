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


        //sets the weapon selected to the current weapon - also enable projection line on new weapon and disable on old
        public void SetWeapon(Weapon weapon)
        {
            if (currentWeapon != weapon)
            {
                if (currentWeapon != null)
                {
                    //turn off the current projection line
                    Projectile_Trajectory temp = currentWeapon.FiringPoint.GetComponent<Projectile_Trajectory>();
                    if (temp != null)
                    {
                        currentWeapon.FiringPoint.GetComponent<LineRenderer>().enabled = false;
                        temp.enabled = false;
                    }
                }
               
                Debug.Log("***WeaponManager Changed weapon to " + weapon.ToString());
                currentWeapon = weapon;
                Projectile_Trajectory currentWeaponTrajectory = currentWeapon.FiringPoint.GetComponent<Projectile_Trajectory>();
                if (currentWeaponTrajectory != null)
                    currentWeaponTrajectory.enabled = true;
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

		public void SwitchAmmo()
		{
			if (!CameraManager.instance.inDefaultPosition)
			{
				if (currentWeapon != null)
				{
					currentWeapon.SwitchAmmo();
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

		//need to utilise smooth movement of the weapon via the weapon obj fixed update and possible lerping
		public void MoveWeaponForward()
		{
			currentWeapon.MoveWeaponForwardBackward("Forward");
		}

		public void MoveWeaponBackward()
		{
			currentWeapon.MoveWeaponForwardBackward("Backward");
		}

		public void MoveWeaponLeft()
		{

		}

		public void MoveWeaponRight()
		{

		}


	}
}
