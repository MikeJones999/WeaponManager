using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;
    public GameObject cameraDefault;
    public bool inDefaultPosition;
    private GameObject CurrentWeaponFocus;
    private bool followingProjectile;
    private bool WeaponAnimInProgress;
    private GameObject projectile;
    public float offset;


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
    }

    /**
     * Moves the camera to the default position when all criteria is met
     */
    private void DefaultPosition()
    {
        inDefaultPosition = true;
        Camera.main.transform.position = cameraDefault.transform.position;
        Camera.main.transform.rotation = cameraDefault.transform.rotation;
        CurrentWeaponFocus = null;
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
                CurrentWeaponFocus = weapon;
                var pos = weapon.transform.position;
                MoveCameraToAWeaponPos(pos);
            }
        }

    }

    /**
     * This Method refocuses the camera following a projectile being shot - focuses back onto the weapon taht fired it
     * */
    public void ReFocusMe(GameObject weapon)
    {
        CurrentWeaponFocus = weapon;        
        var pos = weapon.transform.position;
        MoveCameraToAWeaponPos(pos);
    }

    /**
     * Helper method to reduced duplication - simply moves camera to vector3 provided with offset
     * */
    private void MoveCameraToAWeaponPos(Vector3 pos)
    { 
        inDefaultPosition = false;
        var offset = new Vector3(pos.x - 5.5f, pos.y + 1.5f, pos.z - 3);

        Camera.main.transform.position = offset;
        Camera.main.transform.LookAt(pos);
    }


    // Use this for initialization
    void Start () {

        inDefaultPosition = true;
        Camera.main.transform.position = cameraDefault.transform.position;
       // Camera.main.transform.rotation = cameraDefault.transform.rotation;
    }


    /**
     * Moves the camera to the default position (defined by an empty 3d object in game)
     * Only moves to default if not following projectile or a weapon's animation is not being played
     * This method is the public method for calling the actual method that moves the camera
     * */
    public void MoveToDefaultPos()
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


    /**
     * Follows the projectile that has most recently been shot
     * Once shot the responsibility of the collision lays with teh projectile object and not the camera manager
     * 
     * */
    public void FollowFiredProjectile(GameObject Ammo)
    {
        projectile = Ammo;
        followingProjectile = true;
    }

    /**
     *Stops the camera from following teh most recently shot projectile 
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

}
