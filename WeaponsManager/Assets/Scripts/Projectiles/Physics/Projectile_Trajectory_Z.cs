
using UnityEngine;


namespace Assets.Scripts.Projectiles.Physics
{
	class Projectile_Trajectory_Z : Projectile_Trajectory
    {

		public override void CalculateTrajectory()
		{

			//var angle = loadingpoint.transform.rotation.eulerAngles.z;
			//Debugger.Trace("Weapon_Catapult.cs - Angle of Projectile: " + angle.ToString());

			float vy = (force / objMass) * Mathf.Sin((angle * Mathf.Deg2Rad));
			float vz = (force / objMass) * Mathf.Cos((angle * Mathf.Deg2Rad));
			float dx = loadingpoint.transform.position.x;
			float dy = loadingpoint.transform.position.y;
			float dz = loadingpoint.transform.position.z;
			//float x = 0;

			//	AmmoProjectile.transform.rotation = AmmoProjectile.transform.parent.rotation;

			for (int i = 0; i < lineRes; i++)
			{

				if (i == 0)
				{

					dx = 0;
					dy = 0;
					dz = 0;

				}
				else
				{

					vy = vy - (grav / 2) * dt; //Time.deltaTime;
					dz = dz + vz * dt; //Time.deltaTime;
					dy = dy + vy * dt; //Time.deltaTime;
				}
				lr.SetPosition(i, new Vector3(dx, dy, dz));
			}

			lr.useWorldSpace = false;

		}

	}
}
