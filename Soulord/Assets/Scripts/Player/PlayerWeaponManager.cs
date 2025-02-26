using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }
    public Weapon currentWeapon;
    public Weapon secondaryWeapon;

    private void Update()
    {
        if ( Input.GetMouseButtonDown(0) && currentWeapon != null )
        {
            currentWeapon.Attack();
        }

        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            SwapWeapon();
        }
    }

    public void SwapWeapon()
    {
        Weapon temp = currentWeapon;
        currentWeapon = secondaryWeapon;
        secondaryWeapon = temp;
    }
    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickupWeapon(Weapon weapon)
    {
        if ( currentWeapon != null )
        {
            Destroy(currentWeapon); // Xóa vũ khí cũ trước khi nhặt vũ khí mới
        }

        currentWeapon = Instantiate(weapon , transform.position , Quaternion.identity , transform);
    }
}
