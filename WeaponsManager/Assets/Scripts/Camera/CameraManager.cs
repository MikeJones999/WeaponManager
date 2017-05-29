using System;
using System.Collections.Generic;
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
	public float StrafingOffsetCamX;
	private List<Camera> ListAllCameras;
	private float fieldOfViewDefault;
	public float fieldOfViewWeapon;
	public bool CameraStrafing;
	protected bool DefaultCamHasStraffed;

	private bool followingWeaponDuringMove;

	#region "Lerping Movement"
	// The time taken to move from the start to finish positions
	public float timeTakenDuringLerp = 1f;

	// How far the object should move when 'space' is pressed
	private float distanceToMove;

	//Whether we are currently interpolating or not
	private bool _isLerping;

	//The start and finish positions for the interpolation
	private Vector3 _startPosition;
	private Vector3 _endPosition;

	//The Time.time value when we started the interpolation
	private float _timeStartedLerping;

	#endregion




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

	private void ReturnToDefaultPosition()
	{
		//we should now be in default position
		inDefaultPosition = true;
		_endPosition = cameraDefault.transform.position;
		var pos = CurrentWeaponFocus.transform.position;
		var CamOffset = new Vector3(pos.x - offsetCamX, pos.y + offsetCamY, pos.z);
		
		_startPosition = CamOffset;
		//Camera.main.transform.rotation = cameraDefault.transform.rotation;
		Camera.main.fieldOfView = fieldOfViewDefault;
		CurrentWeaponFocus = null;
		ShowWeaponUI(false);
		StartLerping();

	}

	//Use this Method to Hide User Interface elements when in default position
	void ShowWeaponUI(bool status)
	{
		//going into default view
		if (!status)
		{
			foreach (Camera cam in ListAllCameras)
			{
				if (cam.CompareTag("CameraInWeaponMode") )
				{
					cam.gameObject.SetActive(status);
				}

				if (cam.CompareTag("CameraMovementDir"))
				{
					if (inDefaultPosition)
					{
						cam.gameObject.SetActive(true);
					}else
					{
						cam.gameObject.SetActive(false);
					}
				}
			}
		}
		else  //Moving into weapon view 
		{
			//Check current weapon's movement capabilities
			bool LeftAndRight = CurrentWeaponFocus.GetComponent<Weapon>().LeftAndRight;
			bool FowardAndBackward = CurrentWeaponFocus.GetComponent<Weapon>().FowardAndBackward;

			foreach (Camera cam in ListAllCameras)
			{
				if (cam.CompareTag("CameraInWeaponMode"))
				{
					if (FowardAndBackward && cam.name.Equals("UiCameraFB"))
					{
						cam.gameObject.SetActive(status);
					}
				}

				if (cam.CompareTag("CameraMovementDir"))
				{
					cam.gameObject.SetActive(false);
				}
			
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
				CurrentWeaponFocus = weapon;			
				ShowWeaponUI(true);			
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
		//pos = weapons position to move to
		inDefaultPosition = false;
		ShowWeaponUI(true);
		//changed this line from cameradefault - to the camera itself so that this method can be used when moving from projectile to weapon
		_startPosition = Camera.main.transform.position;

		var CamOffset = new Vector3(pos.x - offsetCamX, pos.y + offsetCamY, pos.z);

		_endPosition = CamOffset;


		//Camera.main.transform.position = CamOffset;
		//Camera.main.transform.LookAt(pos);

		StartLerping();

	}


	// Use this for initialization
	void Start () {

		SetUpDefaultFieldOfView();
		DefaultPosition();

		offsetCamX = 3.45f;
		offsetCamY = 1.90f;
		StrafingOffsetCamX = 4.00f;
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
				//DefaultPosition();
				ReturnToDefaultPosition();

			}
			else
			{
				Debug.Log("Cannot change to default view - as following Projectile");
			}
		}
		else
		{
			if (Camera.main.transform.position != cameraDefault.transform.position)
			{
				StrafeCamera("DefaultPos");
			}
			else
			{
				Debug.Log("Already in default view");
			}
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
		//make the projectile null as it should now be deleted by the weapons manager and will no longer exist.
		projectile = null;
		followingProjectile = false;
		//Make a call to move camera back to weapon focus
		MoveCameraToAWeaponPos(CurrentWeaponFocus.transform.position);
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

		if(!followingProjectile && !WeaponAnimInProgress && followingWeaponDuringMove)
		{
			if (CurrentWeaponFocus != null)
			{
				Vector3 CurrentWeaponPos = CurrentWeaponFocus.transform.position;
				Camera.main.transform.position = new Vector3(CurrentWeaponPos.x - offsetCamX, CurrentWeaponPos.y + offsetCamY, CurrentWeaponPos.z);
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


	public void SetFollowingWeaponDuringMove(bool status)
	{
		this.followingWeaponDuringMove = status;
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

	//Handles the UI Left button being pressed
	public void UILeftMovement()
	{
		StrafeCamera("Left");
	}

	//Handles the UI Right button being pressed
	public void UIRightMovement()
	{
		StrafeCamera("Right");
	}

	//Method to handle the strafing of the Main Camera left and right - moves in segments of stipulated offset x
	//Once end position is set - the StartLerping method is called.  
	public void StrafeCamera(string direction)
	{
		if (inDefaultPosition)
		{
			_startPosition = Camera.main.transform.position;
			Vector3 CamOffset = _startPosition;
			if (direction.Equals("Left"))
			{
				CamOffset = new Vector3(_startPosition.x - StrafingOffsetCamX, _startPosition.y, _startPosition.z);
			}
			else if (direction.Equals("Right"))
			{
				CamOffset = new Vector3(_startPosition.x + StrafingOffsetCamX, _startPosition.y, _startPosition.z);
			}
			else if(direction.Equals("DefaultPos"))
			{
				CamOffset = cameraDefault.transform.position;
			}


			_endPosition = CamOffset;

			//Camera.main.transform.position = CamOffset;
			//Camera.main.transform.LookAt(pos);
			CameraStrafing = true;
			StartLerping();
		}
	}

	//We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
	void FixedUpdate()
	{
		if (_isLerping)
		{
			//We want percentage = 0.0 when Time.time = _timeStartedLerping
			//and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
			//In other words, we want to know what percentage of "timeTakenDuringLerp" the value
			//"Time.time - _timeStartedLerping" is.
			float timeSinceStarted = Time.time - _timeStartedLerping;
			float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

			//Perform the actual lerping.  Notice that the first two parameters will always be the same
			//throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
			//to start another lerp)
			Camera.main.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
			//Constantly update looking at the weapon for smooth rotation
			if (!inDefaultPosition)
			{
				Camera.main.transform.LookAt(CurrentWeaponFocus.transform);
			}
			else if (inDefaultPosition && CameraStrafing)
			{
				//stay looking forward
			}
			else
			{
				Camera.main.transform.LookAt(Vector3.zero);
			}
			

			//When we've completed the lerp, we set _isLerping to false
			if (percentageComplete >= 1.0f)
			{			
				_isLerping = false;

				//End the movement of the camera from strafing
				if (inDefaultPosition && CameraStrafing)
				{
					CameraStrafing = false;
				}
			}
		}
	}

	#region "Lerping Methods"

	void StartLerping()
	{
		
		_timeStartedLerping = Time.time;
		//Camera.main.transform.LookAt(_endPosition);

		//We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
		//_startPosition = transform.position;
		//distanceToMove = Vector3.Distance(_startPosition, _endPosition);
		//_endPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceToMove;
		_isLerping = true;
	}

	#endregion


}
