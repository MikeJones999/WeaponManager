using Assets.Scripts.Projectiles;

public class ArcherArrowStd_Projectile : Projectile
{
		void Awake()
		{
			SetProjectileForce(1300);
			isDestroyableAfterImpact = false;
		}

}
