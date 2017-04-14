using Assets.Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Scripts.Projectiles
{
    public abstract class  Projectile :MonoBehaviour
    {
        private int ProjectileForce;
        protected bool isDestroyableAfterImpact { get; set; }


        public int GetProjectileForce()
        {
            return ProjectileForce;
        }

        protected void SetProjectileForce(int force)
        {
            this.ProjectileForce = force;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag != "Weapon")
            {
                Invoke("StopProjectileFromBeingFollowed", 4.0f);                 
            }
        }

        private void StopProjectileFromBeingFollowed()
        {
            CameraManager.instance.StopFollowingFiredProjectile();
            WeaponsManager.instance.DestroyFiredProjectile();
        }

    }
}
