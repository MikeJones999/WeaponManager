using Assets.Scripts.Projectiles;
using System;
using UnityEngine;

public class Weapon_Crossbow : Weapon
{

	public void Awake()
	{
		//specify the movement of this weapon
		LeftAndRight = true;
		FowardAndBackward = true;
		SwitchAmmo();
	}

	public override void Fire()
	{
		if (weaponLoaded)
		{
			firingInProgress = true;
			Debug.Log("Fired : weapon");
		   // WeaponModel.GetComponent<Animator>().Play("FireShot");
			AmmoCount--;
			//weapon is firing - so cannot choose default view or another weapon


			//tell camera manager that Animation has now started - so that Default mode cannot be pressed
			CameraManagerInformAnimationBeingPlayed(true);

			FireProjectile();

		}
		else
		{
			Debug.Log("No ammo loaded - handle with UI notice!!!!");
		}
	}

	public override void FireProjectile()
	{
		//disconnect ammo from parent
		// var temp = AmmoProjectile.transform.parent;
		AmmoProjectile.transform.parent = null;

		//enable the collider on the ammo so it can connect with other objects
		AmmoProjectile.GetComponent<Collider>().enabled = true;


		//initialise rigidbody's gravity
		AmmoProjectile.GetComponent<Rigidbody>().useGravity = true;
		//Get angle, if not 45deg set to 45deg


		//addforce (fire) using ammo's force parameter
		 AmmoProjectile.GetComponent<Rigidbody>().AddForce(-AmmoProjectile.transform.forward * ProjectileForceApplied, ForceMode.Acceleration);
		//AmmoProjectile.transform.Rotate(25, 0, 0);
	  //AmmoProjectile.GetComponent<Rigidbody>().AddForce(new Vector3(1, 0, 0) * ProjectileForceApplied, ForceMode.Force);

	   

		//Weapon shown as not loaded
		weaponLoaded = false;


		//camera follow until object destroyed
		CameraFollowProjectile();

		//tell camera manager that Animation has now stopped - so that it can follow the projectile
		CameraManagerInformAnimationBeingPlayed(false);
	}

	public override void LoadProjectile()
	{
		//test purposes
		//SwitchAmmo(Ammo, 5);

		//if weapon currently doesn't have any projectiles attached then continue to load one
		if ((!weaponLoaded) && (!projectileExists))
		{
			if (Ammo != null && AmmoCount > 0)
			{
				if (AmmoLoadPos != null)
				{
					//There is a a position attached to the weapon that an object can be created at - therefore create a copy of the ammo prefab (attached to this script) and place it at AmmoLoadPos
					AmmoProjectile = Instantiate(Ammo, new Vector3(AmmoLoadPos.transform.position.x, AmmoLoadPos.transform.position.y, AmmoLoadPos.transform.position.z), AmmoLoadPos.transform.rotation) as GameObject;

					//AmmoProjectile.transform.position += -AmmoProjectile.transform.forward * 0.15f;
					//AmmoProjectile.transform.position = new Vector3(AmmoProjectile.transform.localPosition.x + 0.75f, AmmoProjectile.transform.position.y, AmmoProjectile.transform.position.z);


					AmmoProjectile.transform.Rotate(0, -90, 0);

					//Now that the object is created - get the script attached to it called projectile - and then get the force of this particular projectile type. - Projectile is an abstract class. This saves us having to hard code the value each time for different projectiles
					this.ProjectileForceApplied = AmmoProjectile.GetComponent<Projectile>().GetProjectileForce();

					//If the object is actually created then attach it to the AmmoLoadPos - so that it will follow it during any animation
					if (AmmoProjectile != null)
					{
						AmmoProjectile.transform.parent = AmmoLoadPos.transform;
						//Weapon is now loaded so stop it being loaded at the moment
						this.weaponLoaded = true;
						this.projectileExists = true;
					}

				}
				else
				{
					//Error Cant find loading pos
					Debug.Log("Error Cant find loading pos for Ammo - or you have not identified it in the script");
				}

			}
			else
			{
				weaponLoaded = false;
				Debug.Log("No ammo loaded or out of ammo - handle with UI notice!!!!");
			}
		}
		else
		{
			Debug.Log("Weapon already loaded");
		}

	}

	/**
	 * Moves the Stated weapon in the desired way - each object will behave differently hence why this is not inherited
	 */
	public override void SpecificWeaponMovement()
	{
		rotateY += Input.GetAxis("Mouse X") * sensitivity;
		//rotation.y += Input.GetAxis("Mouse X") * sensitivity;

		//added rotation up and down for crossbow
		rotateX += Input.GetAxis("Mouse Y") * sensitivity;

		////Used to set the ball forward on first click
		//if (rotateY == 0.0f)
  //      {
  //          rotateY = 0.0f;
  //      }
		rotateY = Mathf.Clamp(rotateY, -45.0f, 45.0f);

		//added rotation up and down for crossbow - limits the collision with the ground - issue with rigid body
		rotateX = Mathf.Clamp(rotateX, -9.0f, 3.0f);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -rotateY, -rotateX);
		if (weaponLoaded)
		{
			AmmoProjectile.transform.rotation = AmmoLoadPos.transform.rotation;
		    AmmoProjectile.transform.Rotate(0, -90, 0);
		}
	}

	public override void SwitchAmmo()
	{
		//handle initial run and set default ammo
		if (this.Ammo == null)
		{
			this.Ammo = Resources.Load("prefabs/projectiles/BallistSPEARModel_v1") as GameObject;
			this.AmmoCount = 5;
		}
		else
		{

			this.Ammo = Resources.Load("prefabs/projectiles/BallistSPEARModel_v1") as GameObject;
			this.AmmoCount = 5;
		}


	}

	public override string ToString()
		{
			return "WeaponModel: Crossbow";
		}

	//void Update()
	//{
	//	if (isMouseDown)
	//	{
	//		SpecificWeaponMovement();
	//	}
	//}

}