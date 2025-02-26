//using UnityEngine;

//public class WeaponPickup : MonoBehaviour
//{
//    private bool isNearPlayer = false;
//    private Transform player;

//    private void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player")?.transform;
//    }

//    private void Update()
//    {
//        if ( isNearPlayer && Input.GetKeyDown(KeyCode.F) )
//        {
//            PickupWeapon();
//        }
//    }

//    private void PickupWeapon()
//    {
//        PlayerWeaponManager.Instance.PickupWeapon(gameObject);
//        Destroy(gameObject);
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if ( collision.CompareTag("Player") )
//        {
//            isNearPlayer = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if ( collision.CompareTag("Player") )
//        {
//            isNearPlayer = false;
//        }
//    }
//}
