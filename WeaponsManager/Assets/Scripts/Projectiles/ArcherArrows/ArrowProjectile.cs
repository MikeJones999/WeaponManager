using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles.ArcherArrows
{
	public class ArrowProjectile : Projectile
	{


		void FixedUpdate()
		{

			//Handles the tipping of the arrow towards the ground during flight
			Vector3 velocity = transform.GetComponent<Rigidbody>().velocity;
			if (velocity != Vector3.zero)
			{
				transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(velocity);
			}
		}


	}
}
