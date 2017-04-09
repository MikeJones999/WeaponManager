using Assets.Scripts.Weapons;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	private GameObject weapon;

    private int health;

	// Use this for initialization
	void Start () {

        //this transform.gameobject relates to the object in which the script is attached
		weapon = transform.gameObject;			
	}


    public abstract void Fire();

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

	void OnMouseDown()
	{
		CameraManager.instance.FocusMe(weapon);
        WeaponsManager.instance.SetWeapon(this);      
	}

  

}
