using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool gettingKnockBack {  get; private set; }
    [SerializeField] private float knockBackTime = .2f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockBack (Transform damageSrc, float knockBackThrust)
    {
        gettingKnockBack = true;
        Vector2 differance = (transform.position - damageSrc.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce (differance, ForceMode2D.Impulse);
        StartCoroutine(KnockRountine());
    }

    private IEnumerator KnockRountine()
    {
        yield return new WaitForSeconds (knockBackTime);
        rb.linearVelocity = Vector2.zero;
        gettingKnockBack = false;
    }
}
