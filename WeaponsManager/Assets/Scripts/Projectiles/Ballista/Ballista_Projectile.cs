using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Ballista
{
	class Ballista_Projectile : Projectile
	{


		void FixedUpdate()
		{
			if (inFlight)
			{
				//Handles the tipping of the arrow towards the ground during flight
				UnityEngine.Vector3 velocity = transform.GetComponent<Rigidbody>().velocity;
				if (velocity != Vector3.zero)
				{
					transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(velocity);
				}
			}
		}


		//identify to this script that the object is in flight through add force
		public void InFlight()
		{
			this.inFlight = true;
		}


	}
}
