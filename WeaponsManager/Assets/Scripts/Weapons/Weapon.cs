/**
 * Author - Mike Jones
 * Weapons class is an Abstract class that all game objects that are designated weapons will inherit.
 * All Weapons must be given the tag of "weapon";
 * Upon clicking on a weapon model the mouse down function is call which informs the camera Manager to focus upon it.
 * This Class also informs the Camera Manager to follow the projectile that is shot
 * This class handles the destruction of the Projectile - which is  originates from the projectile itself
 * FireProjectile method has to be handles by the weapon model class itself (e.g Weapon_Catapult - which would inherit from this class) as this gets called by the animation
 * ProjectileExists - relates to the projectile that has been loaded and fired. That means if you load another projectile and fire it - it will only happen after the other projectile has been identified as ready for destruction (in memory)
 * 
 * */

using Assets.Scripts.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Weapon : MonoBehaviour {

	private GameObject weapon;
	protected bool isMouseDown;
	private int health;
	protected bool weaponLoaded;
	protected bool projectileExists;
	
	protected static bool firingInProgress;
	protected int AmmoCount;
	protected float ProjectileForceApplied;
	protected GameObject AmmoProjectile;
	public float projectileDestroyDelay;
	public GameObject WeaponModel;
	protected GameObject Ammo;
	public GameObject AmmoLoadPos;
	private bool WeaponIsMoving;
	private bool collidedWithObject;

	enum MoveDirection
	{
		forward,
		backward,
		left,
		right			 
	}

	private MoveDirection dir;


	//Ui Visible in weapon mode - For Movement
	public bool LeftAndRight { get; set; }
	public bool FowardAndBackward {get; set;}
	public Vector3 WeaponStartingPosition;


	protected float rotateX;
	protected float rotateY;
	public float sensitivity;

	// Use this for initialization
	void Start () {

		//this transform.gameobject relates to the object in which the script is attached
		weapon = transform.gameObject;
		weaponLoaded = false;
		projectileExists = false;
		isMouseDown = false;
		//WeaponStartingPosition = transform.position;
		GameManager.instance.AddGameTransform(transform);
	}


	public virtual void Fire()
	{
		if (weaponLoaded && !WeaponIsMoving)
		{
			if (!firingInProgress)
			{
				firingInProgress = true;
				Debug.Log("Fired : weapon");
				WeaponModel.GetComponent<Animator>().Play("FireShot");
				AmmoCount--;
				//weapon is firing - so cannot choose default view or another weapon


				//tell camera manager that Animation has now started - so that Default mode cannot be pressed
				CameraManagerInformAnimationBeingPlayed(true);
			}
			else
			{
				Debug.Log("Currently firing in progress - handle with UI notice!!!!");
			}

		}
		else
		{
			Debug.Log("No ammo loaded - handle with UI notice!!!!");
		}
	}

	public abstract void FireProjectile();


	public void ReduceHealth(int damage)
	{
		health = health - damage;
		if( health <= 0)
		{
			Debug.Log(this.ToString());
		}
	}

	public int GetHealth()
	{
		return health;
	}

	public abstract void LoadProjectile();
	public abstract void SwitchAmmo();


	/**
	 * On mouse clicking on weapon object focus on me - camera manager decides if the view is already current
	 * */
	void OnMouseDown()
	{
		//***MW*** 19/04/2017 Handles the button interaction - if not clicking on ui then call underlining code
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (!firingInProgress)
			{
				if (CameraManager.instance.GetCurrentWeaponFocus() != this.gameObject)
				{
					CameraManager.instance.FocusMe(weapon);
					WeaponsManager.instance.SetWeapon(this);
				}
				else
				{
					isMouseDown = true;
				}
			}
		}
	}

	void OnMouseUp()
	{
		isMouseDown = false;
	}

	/**
	 * Destroy the current projectile
	 * */
	public void DestroyFiredProjectile()
	{
		if (AmmoProjectile != null)
		{            
			Destroy(AmmoProjectile, projectileDestroyDelay);
			projectileExists = false;
		}
	}

	/**
	 * Destroy the passed projectile 
	 * This overload method ensures that the correct projectile is deleted and not any projectile that would follow
	 * */
	public void DestroyFiredProjectile(GameObject projectile)
	{
		if (projectile != null)        {

			Destroy(projectile, projectileDestroyDelay);
			projectileExists = false;
		}
	}

	/**
	 * Requests that the camera manager should follow the projectile
	 * Camera manager handles this and decides if it can or is there is an animation in progress
	 */
	protected void CameraFollowProjectile()
	{
		CameraManager.instance.FollowFiredProjectile(AmmoProjectile);

	}

	protected void CameraManagerInformAnimationBeingPlayed(bool status)
	{      
		CameraManager.instance.SetWeaponAnimInProgress(status);
	}


	public void NoLongerFiringInProgress()
	{
		firingInProgress = false;
	}


	public abstract void SpecificWeaponMovement();


	public void MoveWeaponForwardBackward(string direction)
	{
		WeaponStartingPosition = transform.position;
		WeaponIsMoving = true;
		if(direction.Equals("Forward"))
		{
			dir = MoveDirection.forward;
		}
		else
		{
			dir = MoveDirection.backward;
		}
		//get weapons current distance from starting point (anchored)
		/*
		 * Need to work out how to ascertain if object in in front or behind
		if (Vector3.Distance(WeaponStartingPosition, weapon.transform.position) < 10.0f)
			{
				
			}

		//Can however - use the above method to work out if the object is near the edge of the map - thus restricting it
			*/


		//if not greater than 10 metres from starting point
		//and if not colliding with anything 
		//move forward/backwards depending on the string

		//movement to be handled in FixedUpdate
	}

	public void Update()
	{

		if (isMouseDown)
		{
			SpecificWeaponMovement();
		}


		if (WeaponIsMoving && !isMouseDown && !firingInProgress)
		{
			if (!collidedWithObject)
			{
				if (Vector3.Distance(WeaponStartingPosition, weapon.transform.position) < 3.0f)
				{
					//check for collisions - if collide stop instantly
					CameraManager.instance.SetFollowingWeaponDuringMove(true);
					Debug.Log("Weapon is moving");
					if (dir == MoveDirection.forward)
					{
						weapon.transform.position = new Vector3(weapon.transform.position.x + 0.05f, weapon.transform.position.y, weapon.transform.position.z);
					}
					else if (dir == MoveDirection.backward)
					{
						weapon.transform.position = new Vector3(weapon.transform.position.x - 0.05f, weapon.transform.position.y, weapon.transform.position.z);
					}
				
				}
				else
				{
					WeaponIsMoving = false;
					Debug.Log("Weapon has stopped moving");
					CameraManager.instance.SetFollowingWeaponDuringMove(false);
				}
			}
			else
			{
				//This section handles when a weapon hits a weapon (or later something else in its moving stage)
				//If it hits something then stop the movement process and move it backwards slightly
				WeaponIsMoving = false;
				Debug.Log("Weapon has stopped moving");
				if (dir == MoveDirection.forward)
				{
					weapon.transform.position = new Vector3(weapon.transform.position.x - 0.05f, weapon.transform.position.y, weapon.transform.position.z);
				}
				else if (dir == MoveDirection.backward)
				{
					weapon.transform.position = new Vector3(weapon.transform.position.x + 0.05f, weapon.transform.position.y,
						weapon.transform.position.z);
				}
				
				CameraManager.instance.SetFollowingWeaponDuringMove(false);
				collidedWithObject = false;
			}
			
		}
	
	}

	public void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Weapon"))
		{
			collidedWithObject = true;
		}
	}


}
