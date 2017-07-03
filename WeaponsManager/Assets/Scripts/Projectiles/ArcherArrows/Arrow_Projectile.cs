using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles.ArcherArrows
{
	public class Arrow_Projectile : Projectile
	{
		private List<string> ObjectsToStickTo;
		private bool MainArrow;
		

		private void Start()
		{
			ObjectsToStickTo = new List<string>();
			ObjectsToStickTo.Add("Ground");
			ObjectsToStickTo.Add("Building");
			ObjectsToStickTo.Add("CentrePoint");
		}
		



		void FixedUpdate()
		{
			if (inFlight)
			{
				//Handles the tipping of the arrow towards the ground during flight
				Vector3 velocity = transform.GetComponent<Rigidbody>().velocity;
				if (velocity != Vector3.zero)
				{
					transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(velocity);
				}
			}
		}


		
		


		void OnCollisionEnter(Collision collision)
		{
			if(ObjectsToStickTo.Contains(collision.gameObject.tag))
			{
				this.inFlight = false;
				Vector3 finalPosition = transform.position;
				Rigidbody rb = transform.GetComponent<Rigidbody>();			
				rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
				Destroy(rb);
				transform.GetComponent<Collider>().enabled = false;

				//if its the initial Main arrow then once it has hit an object call the return to weapon. Any arrow that is not the main arrow then ignore this
				if (MainArrow)
				{
					Invoke("StopProjectileFromBeingFollowed", 4.0f);
				}
			}

		}

		public void InFlight()
		{
			this.inFlight = true;
		}

		//Means to assign as the main initial arrow that was fired. This arrow will then act as the point of contact to return to the weapon upon collision
		public void AssignMainArrow()
		{
			this.MainArrow = true;
		}
	}
}
