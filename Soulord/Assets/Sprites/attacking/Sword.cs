using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerControllerZ playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;

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
    }

    void Start()
    {
        playerControls.Combat.MeleeAttack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
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

        if ( mousePos.x < playerScreenPoint.x )
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0 , -180 , angle);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0 , 0 , angle);
        }
    }

    // TODO -> adjust slash direction effect
}
