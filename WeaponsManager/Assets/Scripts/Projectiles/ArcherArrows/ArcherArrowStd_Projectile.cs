using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.ArcherArrows;
using UnityEngine;

public class ArcherArrowStd_Projectile : Arrow_Projectile
{
		void Awake()
		{
			SetProjectileForce(20);
			isDestroyableAfterImpact = false;
		}


	

}
