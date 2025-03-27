using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControllerZ : Singleton<PlayerControllerZ>
{
    [SerializeField] private float moveSpeed = 1f;
    //dash
    [SerializeField] private float dashSpeed = 2f;
    [SerializeField] private TrailRenderer myTrailRenderer;


    //hp
    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    [SerializeField] private Image hpBar;

    //shield
    [SerializeField] private float maxShield = 40f;
    private float currentShield;
    [SerializeField] private Image ShieldBar;
    private float lastDamageTime;
    private bool isRegeneratingShield = false;

    //Stamina
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;
    [SerializeField] private Image StaminaBar;

    private PlayerControls playerControls;
    [HideInInspector] public Coroutine burnEffectCoroutine;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;
    private bool isDashing = false; 
    public bool FacingLeft
    {
        get { return facingLeft; }
        set { facingLeft = value; }
    }
    
    private static bool isPaused = false;

    private CapsuleCollider2D normalCollider;
    private CapsuleCollider2D specialCollider;
    private EnemySpawner enemySpawner;
    private bool hasMoved = false;

    private void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();

        playerControls.PauseControls.BreakContinue.performed += ctx => TogglePause();
        playerControls.Skills.Special.performed += _ => PerformSpecialSkill();
        GetCorrectCollider2D();
    }

    public IEnumerator FlashRedWhileBurning(float burnDuration)
    {
        float elapsedTime = 0f;
        bool isRed = false;

        while (elapsedTime < burnDuration)
        {
            mySpriteRender.color = isRed ? Color.white : Color.red; // Chuyển đổi giữa màu trắng & đỏ
            isRed = !isRed;
            yield return new WaitForSeconds(0.2f); // Nhấp nháy mỗi 0.2s
            elapsedTime += 0.2f;
        }

        mySpriteRender.color = Color.white; // Trả lại màu ban đầu
    }

    private void GetCorrectCollider2D()
    {
        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();

        if ( colliders.Length >= 2 )
        {
            normalCollider = colliders[0];
            specialCollider = colliders[1];
        }

        if ( specialCollider != null )
        {
            specialCollider.enabled = false;
        }
    }

    private void Start()
    {
        currentHp = maxHp;
        currentShield = maxShield;
        currentStamina = maxStamina;
        UpdateShieldBar();
        UpdateHpBar();
        UpdateStaminaBar();
        playerControls.Combat.Dash.performed += _ => Dash();
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        PlayerInput();

        if (!hasMoved && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            hasMoved = true;
            enemySpawner.PlayerMoved();
        }
        //OnTriggerEnter2D(specialCollider);
        //OnTriggerStay2D(specialCollider);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Đã chuyển Scene: " + scene.name);
        hasMoved = false;
        ReconnectEnemySpawner();
    }

    private void ReconnectEnemySpawner()
    {
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
        if (!hasMoved && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            hasMoved = true;
            enemySpawner.PlayerMoved();
        }
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX" , movement.x);
        myAnimator.SetFloat("moveY" , movement.y);
    }

    private void Move()
    {
        if (rb == null) return;
        //rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
        Vector2 scaledMovement = movement * 15f; 

        if (isDashing)
        {
            scaledMovement *= dashSpeed; 
        }
        //Debug.Log($"Scaled Movement: {scaledMovement}");
        rb.MovePosition(rb.position + scaledMovement * Time.fixedDeltaTime);
    }

    private void AdjustPlayerFacingDirection()
    {
        if (Camera.main == null) return;
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if ( mousePos.x < playerScreenPoint.x )
        {
            mySpriteRender.flipX = true;
            FacingLeft = true;
        }
        else
        {
            mySpriteRender.flipX = false;
            FacingLeft = false;
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }
    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeed;
        myTrailRenderer.emitting = false;
        isDashing = false;  
    }

    private void UpdateHpBar()
    {
        if ( hpBar != null )
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void UpdateShieldBar()
    {
        if ( ShieldBar != null )
        {
            ShieldBar.fillAmount = currentShield / maxShield;
        }
    }

    private bool isInvincible = false;

    public virtual void TakeDame(float damage)
    {
        if ( isInvincible ) return;

        lastDamageTime = Time.time;
        if ( currentShield <= 0 )
        {
            currentHp -= damage;
            currentHp = Mathf.Max(currentHp , 0);
            UpdateHpBar();
            Die();
        }
        else
        {
            currentShield -= damage;
            currentShield = Mathf.Max(currentShield , 0);
            UpdateShieldBar();
        }

        if ( !isRegeneratingShield )
        {
            StartCoroutine(RegenerateShield());
        }
    }

    private IEnumerator RegenerateShield()
    {
        isRegeneratingShield = true;

        while ( currentShield < maxShield )
        {
            yield return new WaitForSeconds(1f);

            if ( Time.time - lastDamageTime >= 3f )
            {
                currentShield = maxShield;
                UpdateShieldBar();
                isRegeneratingShield = false;
                yield break;
            }
        }

        isRegeneratingShield = false;
    }

    public void Die()
    {
        if (currentHp <= 0 && enemySpawner != null)
        {
            enemySpawner.GameOver();
        }
    }

    private void UpdateStaminaBar()
    {
        if ( StaminaBar != null )
        {
            StaminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if ( isPaused )
        {
            Time.timeScale = 0;
            FreezeAllAnimations(true);
        }
        else
        {
            Time.timeScale = 1;
            FreezeAllAnimations(false);
        }
    }
    private void PerformSpecialSkill()
    {
        if (myAnimator == null) return;
        if ( !myAnimator.GetCurrentAnimatorStateInfo(0).IsName("special") )
        {
            StartCoroutine(SpecialSkillRoutine());
        }
    }
    private IEnumerator SpecialSkillRoutine()
    {
        if (myAnimator == null || specialCollider == null) yield break;
        myAnimator.SetTrigger("specialTrigger");
        specialCollider.enabled = true;
        isInvincible = true;

        yield return new WaitForSeconds(3f);

        if (myAnimator != null)
        {
            myAnimator.SetTrigger("idleTrigger");
        }
        specialCollider.enabled = false;
        isInvincible = false;
    }

    private void FreezeAllAnimations(bool freeze)
    {
        //Animator[] allAnimators = FindObjectsOfType<Animator>();
        Animator[] allAnimators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach ( Animator anim in allAnimators )
        {
            anim.enabled = !freeze;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (specialCollider != null && specialCollider.enabled && specialCollider.IsTouching(collision))
        {
            if (collision.CompareTag("Slime") || collision.gameObject.CompareTag("SlimeBlueBoss"))
            {
                EnemyAIL enemy = collision.GetComponent<EnemyAIL>();
                if (enemy != null)
                {
                    enemy.TakeDame(3f);
                }
                //Rigidbody2D slimeRigidbody = collision.GetComponent<Rigidbody2D>();
                //if (slimeRigidbody != null)
                //{
                //    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                //    slimeRigidbody.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);
                //}
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (specialCollider != null && specialCollider.enabled && specialCollider.IsTouching(collision))
        {
            if (collision.CompareTag("Slime") || collision.gameObject.CompareTag("SlimeBlueBoss"))
            {
                EnemyAIL enemy = collision.GetComponent<EnemyAIL>();
                if (enemy != null)
                {
                    enemy.TakeDame(3f);
                }
                //Rigidbody2D slimeRigidbody = collision.GetComponent<Rigidbody2D>();
                //if (slimeRigidbody != null)
                //{
                //    Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                //    slimeRigidbody.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);
                //}
            }
        }
    }

}
