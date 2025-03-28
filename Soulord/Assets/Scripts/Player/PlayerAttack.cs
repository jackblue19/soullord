//using UnityEngine;

//public class PlayerAttack : MonoBehaviour
//{
//    public Transform attackPoint;
//    public GameObject bulletPrefab;
//    public float meleeAttackRate = 1.0f; 
//    public float rangedAttackRate = 0.5f; 
//    private float nextAttackTime = 0f;
//    private PlayerWeaponManager weaponManager;

//    private void Start()
//    {
//        weaponManager = GetComponent<PlayerWeaponManager>();
//    }

//    private void Update()
//    {
//        if ( Time.time >= nextAttackTime && Input.GetMouseButtonDown(0) )
//        {
//            Attack();
//        }
//    }

//    private void Attack()
//    {
//        if ( weaponManager.HasMeleeWeapon() )
//        {
//            // Đánh cận chiến
//            MeleeAttack();
//            nextAttackTime = Time.time + 1f / meleeAttackRate;
//        }
//        else if ( weaponManager.HasRangedWeapon() )
//        {
//            // Bắn xa
//            RangedAttack();
//            nextAttackTime = Time.time + 1f / rangedAttackRate;
//        }
//    }

//    private void MeleeAttack()
//    {
//        Debug.Log("Melee Attack!");
//        // => more animation of melee
//    }

//    private void RangedAttack()
//    {
//        Debug.Log("Ranged Attack!");
//        Instantiate(bulletPrefab , attackPoint.position , attackPoint.rotation);
//    }
//}
