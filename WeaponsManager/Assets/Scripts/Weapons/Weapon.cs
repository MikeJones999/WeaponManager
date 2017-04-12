using Assets.Scripts.Weapons;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	private GameObject weapon;

    private int health;
    protected bool weaponLoaded;
    protected int AmmoCount;

    // Use this for initialization
    void Start () {

        //this transform.gameobject relates to the object in which the script is attached
		weapon = transform.gameObject;			
	}


    public abstract void Fire();

    public abstract void FireAmmo();


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

  

}
