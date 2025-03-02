using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float attackRate; 
    public Transform attackPoint; // Điểm xuất phát (cho đạn hoặc tầm đánh cận chiến)

    protected float nextAttackTime = 0f;

    public abstract void Attack();
}
