using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private PolygonCollider2D swordCollider;
    private Rigidbody2D swordRigidbody;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerControllerZ playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;
    public string slashFlip = "SlashChild";

    //// detect double click
    //private float firstLeftClickTime;
    ////[SerializeField]
    //private float timeBetweenLeftClick = 0.2f;
    //private bool isTimeCheckAllowed = true;
    //private int leftClickCount = 0;


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
        swordCollider = GetComponent<PolygonCollider2D>();
        swordCollider.isTrigger = true;
        swordCollider.enabled = false;
    }

    void Start()
    {
        playerControls.Combat.MeleeAttack.started += _ => Attack();
        swordCollider = GetComponent<PolygonCollider2D>();
        swordRigidbody = GetComponent<Rigidbody2D>();
        swordCollider.isTrigger = false;
        swordCollider.enabled = false;
        swordRigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Update()
    {
        MouseFollowWithOffset();
        changeSwordColliderTrigger();
        //if ( Input.GetMouseButtonUp(0) )
        //{
        //    leftClickCount++;
        //}
        //if ( leftClickCount == 1 && isTimeCheckAllowed )
        //{
        //    firstLeftClickTime = Time.time;
        //    StartCoroutine(DectectDoubleLeftClick());
        //}
    }

    //private IEnumerator DectectDoubleLeftClick()
    //{
    //    isTimeCheckAllowed = false;
    //    while ( Time.time < firstLeftClickTime + timeBetweenLeftClick )
    //    {
    //        if ( leftClickCount == 2 )
    //        {
    //            Debug.Log("double clicked!"); //  replace debug by double attacking method
    //            break;
    //        }
    //        yield return new WaitForEndOfFrame();
    //    }
    //    leftClickCount = 0;
    //    isTimeCheckAllowed = true;
    //}

    public void changeSwordColliderTrigger()
    {
        if ( Input.GetMouseButtonDown(0) )  
        {
            swordCollider.enabled = true;
            swordRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        else if ( Input.GetMouseButtonUp(0) )  
        {
            swordCollider.enabled = false;
            swordRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");

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
        //float angle = 0;

        if ( mousePos.x < playerScreenPoint.x )
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0 , -180 , angle);
            //weaponCollider.transform.rotation = Quaternion.Euler(0 , -180 , angle);

        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0 , 0 , angle);
            //weaponCollider.transform.rotation = Quaternion.Euler(0 , 0 , angle);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Slime") )  
        {

            // => remove follow mouse
            Debug.Log("Sword hit the slime!");

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
