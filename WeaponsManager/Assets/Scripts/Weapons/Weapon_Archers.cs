using Assets.Scripts.Debugging;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.ArcherArrows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Archers : Weapon {

	private int totalNumberOfArchers;
	private List<GameObject> ArrowProjectiles;

	public void Awake()
	{
		//specify the movement of this weapon
		LeftAndRight = false;
		FowardAndBackward = true;
		SwitchAmmo();

		//to be implement with increased archers
		totalNumberOfArchers = 3;
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

		//disconnect ammo from parent - however keep the object in memory to align the ball when  firing
		var parentObj = AmmoProjectile.transform.parent;
		//as we are going to rotate the parent obj - will need to rotate it back - thus get its default rotation for later
		var temp = AmmoProjectile.transform.parent.rotation;
		//disconnect from parent
		AmmoProjectile.transform.parent = null;

		//enable the collider on the ammo so it can connect with other objects
		AmmoProjectile.GetComponent<Collider>().enabled = true;


		//initialise rigidbody's gravity
		AmmoProjectile.GetComponent<Rigidbody>().useGravity = true;
		//Get angle, if not 45deg set to 45deg

		//rotate the parent object - as below - cant seem to rotate the projectile the same
		//parentObj.Rotate(-45, 90, 0);

		// AmmoProjectile.transform.Rotate (-45,90,0);


		//addforce (fire) using projectile's parent forward force parameter
		AmmoProjectile.GetComponent<Rigidbody>().AddForce(AmmoProjectile.transform.forward * ProjectileForceApplied, ForceMode.Impulse);

		

		//now rotate parent object back to as its default
		parentObj.rotation = temp;
		//Weapon shown as not loaded
		weaponLoaded = false;

		Arrow_Projectile arrow = AmmoProjectile.GetComponent<Arrow_Projectile>();
		arrow.InFlight();
		arrow.AssignMainArrow();


		//camera follow until object destroyed
		CameraFollowProjectile();

		//tell camera manager that Animation has now stopped - so that it can follow the projectile
		CameraManagerInformAnimationBeingPlayed(false);

		FireArrowsWithSleepCoroutine(AmmoProjectile);
	}


	void FireArrowsWithSleepCoroutine(GameObject AmmoProjectile)
	{
		StartCoroutine((System.Collections.IEnumerator)FireAdditionArrows(AmmoProjectile));

	}



	IEnumerator FireAdditionArrows(GameObject AmmoProjectile)
	{

		int totalNumberOfArchers = 5;

		//GameObject LastArrowFired = AmmoProjectile;

		for (int i = 1; i < totalNumberOfArchers; i++)
		{

			int offsetX = Random.Range(-2, 4);
			int offsetZ = Random.Range(-2, 3);

			GameObject newArrow = Instantiate(Ammo, new Vector3(AmmoLoadPos.transform.position.x + offsetX, AmmoLoadPos.transform.position.y, AmmoLoadPos.transform.position.z + offsetZ), AmmoLoadPos.transform.rotation) as GameObject;



			newArrow.transform.rotation = AmmoLoadPos.transform.rotation;

			//rotate the arrow - remove it created in correct direction
			//newArrow.transform.Rotate(0, +90, 0);

			newArrow.GetComponent<Collider>().enabled = true;


			//initialise rigidbody's gravity
			newArrow.GetComponent<Rigidbody>().useGravity = true;

			newArrow.GetComponent<Rigidbody>().AddForce(newArrow.transform.forward * ProjectileForceApplied, ForceMode.Impulse);
			Arrow_Projectile arrow = newArrow.GetComponent<Arrow_Projectile>();
			arrow.InFlight();
			yield return new WaitForSeconds(0.1f);
		}

	}



	public override void LoadProjectile()
	{

		//test purposes
		// SwitchAmmo(Ammo, 5);

		//if weapon currently doesn't have any projectiles attached then continue to load one
		if ((!weaponLoaded) && (!projectileExists))
		{
			if (Ammo != null && AmmoCount > 0)
			{
				if (AmmoLoadPos != null)
				{
									
						//There is a a position attached to the weapon that an object can be created at - therefore create a copy of the ammo prefab (attached to this script) and place it at AmmoLoadPos
						AmmoProjectile = Instantiate(Ammo, new Vector3(AmmoLoadPos.transform.position.x, AmmoLoadPos.transform.position.y, AmmoLoadPos.transform.position.z), Quaternion.identity) as GameObject;

					AmmoProjectile.transform.rotation = AmmoLoadPos.transform.rotation;
					
					//rotate the arrow - remove it created in correct direction
					//AmmoProjectile.transform.Rotate(0, -90, 0);

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
			Debugger.Trace("Weapon already loaded");
			//Debug.Log("Weapon already loaded");
		}

	}



	public override void SwitchAmmo()
	{
		if (!projectileExists)
		{
			//handle initial run and set default ammo
			if (this.Ammo == null)
			{

				this.Ammo = Resources.Load("prefabs/projectiles/Arrow_Projectile") as GameObject;
				this.AmmoCount = 5;
			}
			else
			{
				this.Ammo = Resources.Load("prefabs/projectiles/Arrow_Projectile") as GameObject;
				this.AmmoCount = 5;
			}
		}
		else
		{
			Debug.Log("Ammo already loaded - ");
			//change this for - swap out of the ammo at its loaded position

		}


	}

	public override string ToString()
	{
		return "WeaponModel: Archers";
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
		if (rotateY == 0.0f)
		{
			rotateY = 0.0f;
		}
		rotateY = Mathf.Clamp(rotateY, 45.0f, 135.0f);

		//added rotation up and down for crossbow - limits the collision with the ground - issue with rigid body
		rotateX = Mathf.Clamp(rotateX, 10.0f, 90.0f);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotateY, transform.localEulerAngles.z );
		AmmoLoadPos.transform.localEulerAngles = new Vector3(-rotateX, AmmoLoadPos.transform.localEulerAngles.y, AmmoLoadPos.transform.localEulerAngles.z); // - rotateX);
		//transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -rotateY, -rotateX);
		//if (weaponLoaded)
		//{
		//	AmmoProjectile.transform.rotation = AmmoLoadPos.transform.rotation;
		//	AmmoProjectile.transform.Rotate(0, -90, 0);
		//}
	}


}
