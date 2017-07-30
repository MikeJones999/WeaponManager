using Assets.Scripts.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Projectiles.Physics
{
	class RotationXCalculator : MonoBehaviour
	{
		public GameObject EmptyLoadObject;
		private float intialAngle;
		private float currentAngle;

		private float worldSpaceRotation;
		private float localRotation;


		public void Start()
		{
			if(EmptyLoadObject != null)
			{
				float angle = -EmptyLoadObject.transform.rotation.eulerAngles.x;
				Debugger.Trace("Weapon_Crossbow - Angle of Projectile: " + angle.ToString());
				intialAngle = angle;
			}
		}


		public void Update()
		{
			float angle = -EmptyLoadObject.transform.rotation.eulerAngles.x;
			Debugger.Trace("Weapon_Crossbow - Angle of Projectile: " + angle.ToString());
			Projectile_Trajectory_Z proj = transform.GetComponent<Projectile_Trajectory_Z>();
			proj.UpdateAngle(angle);
			

		}




	}

}
