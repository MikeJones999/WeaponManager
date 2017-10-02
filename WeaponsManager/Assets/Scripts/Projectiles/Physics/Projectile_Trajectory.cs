using Assets.Scripts.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Projectile_Trajectory : MonoBehaviour
	{
		protected LineRenderer lr;
		public int lineRes = 10;
		protected float grav;
	    public float angle = 45.0f;
	    public float force; // = 1300.0f;
	    public float objMass; // = 60.0f;
		public GameObject loadingpoint;
		public float dt = 0.1f;



		private void Awake()
		{
			grav = Mathf.Abs(Physics2D.gravity.y);
		}

		void Start()
		{
			lr = gameObject.AddComponent<LineRenderer>();

			lr.startWidth = 0.2f;

			lr.positionCount = lineRes;

			lr.startColor = Color.green;

			//lr.useWorldSpace = false;

		   //loadingpoint.transform.Rotate(0, 45, 0);
		   // lr.transform.Rotate(0, 45, 0);
	}


		void FixedUpdate()
		{

	

		//update the line
		CalculateTrajectory();
		   

	    }

        public void UpdateAngle(float angle)
        {
            this.angle = angle;
        }

    public virtual void CalculateTrajectory()
		{

		//var angle = loadingpoint.transform.rotation.eulerAngles.z;
		//Debugger.Trace("Weapon_Catapult.cs - Angle of Projectile: " + angle.ToString());

		float vy = (force /objMass) * Mathf.Sin((angle * Mathf.Deg2Rad));
		float vz = (force/ objMass) * Mathf.Cos((angle * Mathf.Deg2Rad));
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
                dx = dx + vz * dt; //Time.deltaTime;
                dy = dy + vy * dt; //Time.deltaTime;
            }
			lr.SetPosition(i, new Vector3(dx, dy, dz));
		}

		lr.useWorldSpace = false;
		
	}

	}
