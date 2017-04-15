using Assets.Scripts.Weapons;
using UnityEngine;

/**
 * Author - Mike Jones
 * Abstact class that is inherited by all the projectile type that will be shot in the game
 * This class will hold the force that each projectile will have and changes according to the projectile.
 * This class handles the collision events of the projectile once fired. - Upon collision it will inform the camera manager to stop following
 * And inform the Weapons manager that it can be destroyed. The destruction is handles by the weapons manager
 * 
 * */
namespace Assets.Scripts.Projectiles
{
    public abstract class  Projectile :MonoBehaviour
    {
        private int ProjectileForce;
        protected bool isDestroyableAfterImpact { get; set; }

        /**
         * Gets the projectile force that has been applied to this specific Projectile type
         * */
        public int GetProjectileForce()
        {
            return ProjectileForce;
        }

        /**
        * Sets the projectile force that has been applied to this specific Projectile type
        * can only be set by the inherited projectile - e.g a catapultball...
        * */
        protected void SetProjectileForce(int force)
        {
            this.ProjectileForce = force;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag != "Weapon")
            {
                //call the stated function after the given time - the time should be a global variable that we decide upon
                Invoke("StopProjectileFromBeingFollowed", 4.0f);                 
            }
        }

        /**
         * Calls the camera manager to stop following the projectile and return to the weapon that shot this projectile 
         * Calls the weapon manager and gets it to destroy this projectile once complete
         */
        private void StopProjectileFromBeingFollowed()
        {
            CameraManager.instance.StopFollowingFiredProjectile();
            WeaponsManager.instance.DestroyFiredProjectile();
        }

    }
}
