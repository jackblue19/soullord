using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerZ : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

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
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private bool facingLeft = false;
    public bool FacingLeft
    {
        get { return facingLeft; }
        set { facingLeft = value; }
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        currentHp = maxHp;
        currentShield = maxShield;
        currentStamina = maxStamina;
        UpdateShieldBar();
        UpdateHpBar();
        UpdateStaminaBar();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
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
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
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

    private void UpdateHpBar ()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void UpdateShieldBar()
    {
        if (ShieldBar != null)
        {
            ShieldBar.fillAmount = currentShield / maxShield;
        }
    }

    public virtual void TakeDame(float damage)
    {
        lastDamageTime = Time.time;
        if (currentShield <= 0)
        {
            currentHp -= damage;
            currentHp = Mathf.Max(currentHp, 0);
            UpdateHpBar();
            if (currentHp <= 0)
            {
                Die();
            }
        }
        else
        {
            currentShield -= damage;
            currentShield = Mathf.Max(currentShield, 0);
            UpdateShieldBar();
        }

        if (!isRegeneratingShield)
        {
            StartCoroutine(RegenerateShield());
        }

    }

    private IEnumerator RegenerateShield()
    {
        isRegeneratingShield = true;

        while (currentShield < maxShield)
        {
            yield return new WaitForSeconds(1f);

            if (Time.time - lastDamageTime >= 3f)
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
        Destroy(gameObject);
    }

    private void UpdateStaminaBar()
    {
        if (StaminaBar != null)
        {
            StaminaBar.fillAmount = currentStamina / maxStamina;
        }
    }
}
