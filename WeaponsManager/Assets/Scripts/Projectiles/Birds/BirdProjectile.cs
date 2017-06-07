using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
	abstract class BirdProjectile : Projectile
	{
		protected Dictionary<int, Vector3> waypoints;
		public int waypointCount;
		protected float waypointContact = 1.0f;

		void Awake()
		{
			SetProjectileForce(10);
			isDestroyableAfterImpact = false;
			isBirdProjectile = true;
			inFlight = false;
			waypoints = GameManager.instance.FlightWayPoints;
			waypointCount = 1;
			waypoints.Add(7, GameManager.instance.ReturnGameObject("BirdMan").transform.position);
		}
		public abstract void Fly();

		public void Update()
		{
			if (inFlight)
			{
				float distance = Vector3.Distance(waypoints[waypointCount], transform.position);

				transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointCount], Time.deltaTime * ProjectileForce);
				transform.LookAt(waypoints[waypointCount]); 

				if(distance <= waypointContact)
				{
					waypointCount++;
				}

				if(waypointCount > 7)
				{
					inFlight = false;
					Invoke("StopProjectileFromBeingFollowed", 4.0f);
				}
			}
		}
	}
}
