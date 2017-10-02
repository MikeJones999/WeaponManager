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
                TurnOnOffProjectileFromPreviousWeapon(false);

                Debug.Log("***WeaponManager Changed weapon to " + weapon.ToString());
                currentWeapon = weapon;
                TurnOnOffProjectileFromPreviousWeapon(true);          
            }
            else
            {

                TurnOnOffProjectileFromPreviousWeapon(true);
            }
        }
        
        //turn on / off the current projection line and line renderer
        public void TurnOnOffProjectileFromPreviousWeapon(bool status)
        {
            if (currentWeapon != null)
            {
               
                Projectile_Trajectory temp = currentWeapon.FiringPoint.GetComponent<Projectile_Trajectory>();
                if (temp != null)
                {
                    LineRenderer line = currentWeapon.FiringPoint.GetComponent<LineRenderer>();
                    if (line != null)
                    {
                        line.enabled = status;
                    }
                    temp.enabled = status;
                }
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
