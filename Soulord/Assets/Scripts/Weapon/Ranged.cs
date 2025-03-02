//using UnityEngine;

//public class Ranged : Weapon
//{
//    public GameObject bulletPrefab;
//    public float bulletSpeed = 10f;
//    public int manaCost = 5; 

//    public override void Attack()
//    {
//        if ( Time.time < nextAttackTime ) return;
//        nextAttackTime = Time.time + 1f / attackRate;

//        PlayerMana playerMana = FindObjectOfType<PlayerMana>();
//        if ( playerMana.currentMana < manaCost ) return; // out of mana

//        playerMana.UseMana(manaCost);

//        GameObject bullet = Instantiate(bulletPrefab , attackPoint.position , Quaternion.identity);
//        bullet.GetComponent<Rigidbody2D>().linearVelocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - attackPoint.position).normalized * bulletSpeed;
//    }
//}
