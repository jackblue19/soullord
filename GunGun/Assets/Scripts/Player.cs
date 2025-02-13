using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image HpBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
    }

    void Start()
    {
        currentHp = maxHp;
        updateHpBar();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

    void movePlayer()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;

        //animation chay
        if (playerInput != Vector2.zero) animator.SetBool("isRun", true);
        else animator.SetBool("isRun", false);

        //lat nv
        if (playerInput.x<0) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
    public virtual void TakeDame(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        updateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void updateHpBar()
    {
        if (HpBar != null)
        {
            HpBar.fillAmount = currentHp / maxHp;
        }
    }
}
