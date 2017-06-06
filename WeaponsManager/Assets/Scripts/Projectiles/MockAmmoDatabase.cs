using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Projectiles
{
	class MockAmmoDatabase
	{
		public static List<Projectile>  GetAmmoTypes(string weaponName)
		{
			List<Projectile> projectiles = null;
			if(!String.IsNullOrEmpty(weaponName))
			{
				switch(weaponName)
				{
					case "Catapult": projectiles = new List<Projectile>() { new CatapultBall() }; break;
					case "BALLISTA": projectiles = new List<Projectile>() { new BallistaSpear() }; break;
				}
			}
			else
			{
				Debug.WriteLine("Error - pass empty/ null string to AmmoDB");
			}

			return projectiles;

		}



	}
}
