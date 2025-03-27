using System.Collections;
using UnityEngine;

public class EnemyBossBullet : MonoBehaviour
{
    private float damage;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float burnDamage = 2f;
    [SerializeField] private float burnDuration = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControllerZ player = collision.GetComponent<PlayerControllerZ>();
            if (player != null)
            {
                player.TakeDame(damage);
                player.StartCoroutine(BurnEffect(player));
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator BurnEffect(PlayerControllerZ player)
    {
        float elapsedTime = 0;

        if (player.burnEffectCoroutine != null)
        {
            player.StopCoroutine(player.burnEffectCoroutine);
        }

        player.burnEffectCoroutine = player.StartCoroutine(player.FlashRedWhileBurning(burnDuration));

        while (elapsedTime < burnDuration)
        {
            player.TakeDame(burnDamage);
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

}
