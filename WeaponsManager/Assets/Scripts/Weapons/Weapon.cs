using Assets.Scripts.Weapons;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	private GameObject weapon;

    private int health;
    protected bool weaponLoaded;
    protected int AmmoCount;
    protected float ProjectileForceApplied;
    protected GameObject AmmoProjectile;
    public float projectileDestroyDelay;

    // Use this for initialization
    void Start () {

        //this transform.gameobject relates to the object in which the script is attached
		weapon = transform.gameObject;	
        		
	}


    public abstract void Fire();

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

    public abstract void LoadAmmo();
    public abstract void SwitchAmmo(GameObject ammo, int ammoCount);



    void OnMouseDown()
	{
		CameraManager.instance.FocusMe(weapon);
        WeaponsManager.instance.SetWeapon(this);      
	}


    public void DestroyFiredProjectile()
    {
        if (AmmoProjectile != null)
        {
            Destroy(AmmoProjectile, projectileDestroyDelay);
        }
    }

    protected void CameraFollowProjectile()
    {
        CameraManager.instance.FollowFiredProjectile(AmmoProjectile);

    }


}
