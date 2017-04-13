using UnityEngine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;
    public GameObject cameraDefault;
    public bool inDefaultPosition;
    private GameObject CurrentWeaponFocus;
    private bool followingProjectile;
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

    private void DefaultPosition()
    {
        inDefaultPosition = true;
        Camera.main.transform.position = cameraDefault.transform.position;
        Camera.main.transform.rotation = cameraDefault.transform.rotation;
        CurrentWeaponFocus = null;
    }

    public void FocusMe(GameObject weapon)
    {
        CurrentWeaponFocus = weapon;
        inDefaultPosition = false;
        var pos = weapon.transform.position;

        var offset = new Vector3(pos.x -5.5f, pos.y +1.5f, pos.z -3);

        Camera.main.transform.position = offset;
        Camera.main.transform.LookAt(pos);

    }


    // Use this for initialization
    void Start () {

        inDefaultPosition = true;
        Camera.main.transform.position = cameraDefault.transform.position;
       // Camera.main.transform.rotation = cameraDefault.transform.rotation;
    }

    public void MoveToDefaultPos()
    {
        DefaultPosition();
    }

    public void FollowFiredAmmo(GameObject Ammo)
    {
        projectile = Ammo;
        followingProjectile = true;
    }

    public void StopFollowingFiredAmmo()
    {
        followingProjectile = false;
        FocusMe(CurrentWeaponFocus);
    }

    public void Update()
    {
        if (followingProjectile)
        {
            Vector3 projectilePos = projectile.transform.position;
            Camera.main.transform.position = new Vector3(projectilePos.x - offset, projectilePos.y + 2, projectilePos.z);
        }
    }

}
