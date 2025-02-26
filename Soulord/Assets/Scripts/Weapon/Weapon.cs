using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage;
    public float attackRate; // Tốc độ đánh
    public Transform attackPoint; // Điểm xuất phát (cho đạn hoặc tầm đánh cận chiến)

    protected float nextAttackTime = 0f;

    public abstract void Attack();
}
