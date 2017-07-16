using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	class Projectile_Trajectory : MonoBehaviour
	{
		private LineRenderer lr;
		public int lineRes = 10;
		private float grav;
		public float angle = 45.0f;
		public float force = 1300.0f;
		public float objMass = 60.0f;
		public GameObject loadingpoint;
		public float dt = 0.1f;



		private void Awake()
		{
			grav = Mathf.Abs(Physics.gravity.y);
		}

		void Start()
		{
			lr = gameObject.AddComponent<LineRenderer>();

			lr.startWidth = 0.2f;

			lr.numPositions = lineRes;

			lr.startColor = Color.green;
		}


		void Update()
		{
			//update the line
			CalculateTrajectory();

		}

		void CalculateTrajectory()
		{
		float vx = (force / objMass) * Mathf.Sin((angle * Mathf.Deg2Rad));
		float vy = (force / objMass) * Mathf.Cos((angle * Mathf.Deg2Rad));
			float dx = loadingpoint.transform.position.x;
			float dy = loadingpoint.transform.position.y;
			float z = loadingpoint.transform.position.z;

			for (int i = 0; i < lineRes; i++)
			{

			if (i == 0)
			{


			}
			else
			{


				//vx remains constant
				vy = vy - grav * dt;


				dx = dx + vx * dt;
				dy = dy + vy * dt;
			}
				lr.SetPosition(i, new Vector3(dx, dy, z));
			}
		}

	}
