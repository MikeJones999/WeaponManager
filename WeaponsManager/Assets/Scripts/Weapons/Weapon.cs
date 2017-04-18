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

public abstract class Weapon : MonoBehaviour {

	private GameObject weapon;

    private int health;
    protected bool weaponLoaded;
    protected bool projectileExists;
    protected static bool firingInProgress;
    protected int AmmoCount;
    protected float ProjectileForceApplied;
    protected GameObject AmmoProjectile;
    public float projectileDestroyDelay;
    public GameObject WeaponModel;
    public GameObject Ammo;
    public GameObject AmmoLoadPos;

    // Use this for initialization
    void Start () {

        //this transform.gameobject relates to the object in which the script is attached
		weapon = transform.gameObject;
        weaponLoaded = false;
        projectileExists = false;
        

}


    public virtual void Fire()
    {
        if (weaponLoaded)
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
    public abstract void SwitchAmmo(GameObject ammo, int ammoCount);



    void OnMouseDown()
	{
        if (!firingInProgress)
        {
            CameraManager.instance.FocusMe(weapon);
            WeaponsManager.instance.SetWeapon(this);
        }
	}


    public void DestroyFiredProjectile()
    {
        if (AmmoProjectile != null)
        {            
            Destroy(AmmoProjectile, projectileDestroyDelay);
            projectileExists = false;
        }
    }

    public void DestroyFiredProjectile(GameObject projectile)
    {
        if (projectile != null)        {

            Destroy(projectile, projectileDestroyDelay);
            projectileExists = false;
        }
    }

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


}
