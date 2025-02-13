using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.1f;
    private float nextShot;
    [SerializeField] private int maxAmmo = 24;
    private bool isReloading = false;
    public int currentAmmo;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        RotateGun();
        Shoot();
        Reload();
    }

    void RotateGun()
    {
        if (Input.mousePosition.x<0 || Input.mousePosition.y < 0 || Input.mousePosition.x>Screen.width || Input.mousePosition.y > Screen.height)
        {
            return;
        }
        //xoay sung
        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg; 
        transform.rotation=Quaternion.Euler(0,0, angle + rotateOffset);
        //lat sung
        if(angle< -90 || angle > 90) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(1, -1, 1);
    }
    void Shoot()
    {
        //0 ~ chuot trai
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot && !isReloading)
        {
            nextShot = Time.time+shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
        }
    }

    void Reload()
    {
        if(currentAmmo<maxAmmo && Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            Invoke("FinishReload", 2f);
        }
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
