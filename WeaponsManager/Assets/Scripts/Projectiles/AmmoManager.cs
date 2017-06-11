using Assets.Scripts.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour {


	public static AmmoManager instance;
	private Weapon CurrentWeapon;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}


	public void SetupAmmoForWeapon(Weapon weapon)
	{
		this.CurrentWeapon = weapon;

		//Get Ammo list from DB or equivalent - mock object used for now
		List<Projectile> projectiles = MockAmmoDatabase.GetAmmoTypes(weapon.name);

		//Create and Populate the UI with List<Projctiles>()

	}

	private void SetUpUIWithAmmoTypesAndCount()
	{

	}

}
