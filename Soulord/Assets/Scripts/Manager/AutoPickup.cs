//using UnityEngine;

//public class AutoPickup : MonoBehaviour
//{
//    private Transform player;
//    public float pickupRange = 1.5f;

//    private void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player")?.transform;
//    }

//    private void Update()
//    {
//        if ( player == null ) return;

//        float distance = Vector2.Distance(transform.position , player.position);
//        if ( distance <= pickupRange )
//        {
//            PickupItem();
//        }
//    }

//    private void PickupItem()
//    {
//        if ( gameObject.CompareTag("Mana") )
//        {
//            PlayerStats.Instance.AddMana(5); // Hồi 5 mana
//        }
//        else if ( gameObject.CompareTag("Gold") )
//        {
//            PlayerStats.Instance.AddGold(10); // Nhận 10 vàng
//        }

//        Destroy(gameObject);
//    }
//}
