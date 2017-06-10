using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Projectiles.Birds
{
	class CrowProjectile : BirdProjectile
	{

		private void Start()
		{
			SetProjectileForce(15);
		}

		public override void Fly()
		{
			inFlight = true;
		}
	}
}
