using UnityEngine;

public class EnemyHealthzz : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;

    private int currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Debug.Log(currentHealth);
        DetectDeath();
    }

    private void DetectDeath()
    {
        if ( currentHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
