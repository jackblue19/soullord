using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float damage = 10f;
    private Rigidbody2D swordRigidbody;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerControllerZ playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;
    public string slashFlip = "SlashChild";

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerControllerZ>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.MeleeAttack.started += _ => Attack();
        swordRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }


    private void Attack()
    {
        //why??
        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Attack");
        }

        slashAnim = Instantiate(slashAnimPrefab , slashAnimSpawnPoint.position , Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    public void SwingUpFlipAnim()
    {
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180 , 0 , 0);

        if ( playerController.FacingLeft )
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnim()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0 , 0 , 0);

        if ( playerController.FacingLeft )
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y , mousePos.x) * Mathf.Rad2Deg;

        if ( mousePos.x < playerScreenPoint.x )
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0 , -180 , angle);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0 , 0 , angle);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Slime") )  
        {

            // => remove follow mouse

            EnemyAIL enemy = collision.gameObject.GetComponent<EnemyAIL>();
            if (enemy != null)
            {
                //Debug.Log("Sword hit the slime! "+ damage);
                enemy.TakeDame(damage);
            }
            Rigidbody2D slimeRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if ( slimeRigidbody != null )
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                slimeRigidbody.AddForce(knockbackDirection * 10f , ForceMode2D.Impulse);  
            }

            //collision.gameObject.GetComponent<EnemyAI>().TakeDamage(10); 
        }
    }
    // TODO -> adjust slash direction effect
}
