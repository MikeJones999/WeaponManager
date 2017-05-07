﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;
    public GameObject cameraDefault;
    public bool inDefaultPosition;
    private GameObject CurrentWeaponFocus;
    private bool followingProjectile;
    private bool WeaponAnimInProgress;
    private GameObject projectile;
    public float offset;
	public float offsetCamX;
	public float offsetCamY;
	private List<Camera> ListAllCameras;
	private float fieldOfViewDefault;
	public float fieldOfViewWeapon;


	void Awake()
    {
        if(instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(this);
        }


		if (ListAllCameras == null)
		{
			ListAllCameras = new List<Camera>(Camera.allCameras);
		}
	}

    /**
     * Moves the camera to the default position when all criteria is met
     */
    private void DefaultPosition()
    {
        inDefaultPosition = true;
        Camera.main.transform.position = cameraDefault.transform.position;
        Camera.main.transform.rotation = cameraDefault.transform.rotation;
		Camera.main.fieldOfView = fieldOfViewDefault;
        CurrentWeaponFocus = null;
		ShowWeaponUI(false);

	}

	//Use this Method to Hide User Interface elements when in default position
	//**NOT YET IMPLEMENTED***
	void ShowWeaponUI(bool status)
	{
		foreach (Camera cam in ListAllCameras)
		{
			if (cam.CompareTag("CameraInWeaponMode"))
			{
				cam.gameObject.SetActive(status);				
			}
		}
	}

    /**
     * Focuses the Camera to the weapon that has been clicked on - if the weapon is already the current weapon then do nothing
     * The camera will not focus on a new weapon if the camera is following any projectile or if a weapon's animation is being played
     * */
    public void FocusMe(GameObject weapon)
    {
        //only focus on weapon if - not following projectile and or no animation of a weapon is being played
        if (!followingProjectile && !WeaponAnimInProgress)
        {
            if (CurrentWeaponFocus != weapon)
            {
				//CurrentWeaponFocus = weapon;

				//var pos = weapon.transform.position;

				//var offset = new Vector3(pos.x - 5.5f, pos.y + 1.5f, pos.z - 3);

				//Camera.main.transform.position = offset;
				//Camera.main.transform.LookAt(pos);
				ShowWeaponUI(true);
				CurrentWeaponFocus = weapon;
                var pos = weapon.transform.position;
                MoveCameraToAWeaponPos(pos);
				Camera.main.fieldOfView = fieldOfViewWeapon;
            }
            else
            {
                Debug.Log("Weapon already selected");
            }
        }

    }

    /**
     * This Method refocuses the camera following a projectile being shot - focuses back onto the weapon that fired it
     * */
    public void ReFocusMe(GameObject weapon)
    {
        CurrentWeaponFocus = weapon;        
        var pos = weapon.transform.position;
        MoveCameraToAWeaponPos(pos);
		//show UI for weapon movement
		ShowWeaponUI(true);

	}

    /**
     * Helper method to reduced duplication - simply moves camera to vector3 provided with offset
     * */
    private void MoveCameraToAWeaponPos(Vector3 pos)
    { 
        inDefaultPosition = false;
		//        var offset = new Vector3(pos.x - 5.5f, pos.y + 1.5f, pos.z - 3);
		var CamOffset = new Vector3(pos.x - offsetCamX, pos.y + offsetCamY, pos.z);

		Camera.main.transform.position = CamOffset;
        Camera.main.transform.LookAt(pos);
    }


    // Use this for initialization
    void Start () {

		SetUpDefaultFieldOfView();
		DefaultPosition();

		offsetCamX = 3.45f;
		offsetCamY = 1.90f;
		//Camera.main.transform.position = cameraDefault.transform.position;
		// Camera.main.transform.rotation = cameraDefault.transform.rotation;

		//Get a list of all cameras now - as all camera will only return enabled camera
	
	}


    /**
     * Moves the camera to the default position (defined by an empty 3d object in game)
     * Only moves to default if not following projectile or a weapon's animation is not being played
     * This method is the public method for calling the actual method that moves the camera
     * */
    public void MoveToDefaultPos()
    {
        if (!inDefaultPosition)
        {
            if (!followingProjectile && !WeaponAnimInProgress)
            {
                DefaultPosition();
            }
            else
            {
                Debug.Log("Cannot change to default view - as following Projectile");
            }
        }
        else
        {
            Debug.Log("Already in default view");
        }
    }


    /**
     * Follows the projectile that has most recently been shot
     * Once shot the responsibility of the collision lays with the projectile object and not the camera manager
     * 
     * */
    public void FollowFiredProjectile(GameObject Ammo)
    {
		ShowWeaponUI(false);
		projectile = Ammo;
        followingProjectile = true;
    }

    /**
     *Stops the camera from following the most recently shot projectile 
     */
    public void StopFollowingFiredProjectile()
    {
        followingProjectile = false;
        ReFocusMe(CurrentWeaponFocus);
    }

    public void Update()
    {
        if (followingProjectile && !WeaponAnimInProgress)
        {			
			if (projectile != null)
            {
                Vector3 projectilePos = projectile.transform.position;
                Camera.main.transform.position = new Vector3(projectilePos.x - offset, projectilePos.y + 2, projectilePos.z);
            }
        }
    }

    /**
     *Set the WeaponAnimInProgress marker to true or false
     * This method simply informs the camera manager that an animation is either being player or not played.
     * This then will determine if the camera can move in relation to other events
     * 
     * */
    public void SetWeaponAnimInProgress(bool status)
    {
        this.WeaponAnimInProgress = status;
    }

    public GameObject GetCurrentWeaponFocus()
    {
        if (CurrentWeaponFocus != null)
        {
            return CurrentWeaponFocus;
        }
        return null;
    }

	private void SetUpDefaultFieldOfView()
	{
		fieldOfViewWeapon = 75.0f;
		fieldOfViewDefault = Camera.main.fieldOfView;
	}


}
