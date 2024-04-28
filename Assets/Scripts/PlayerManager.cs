using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    
    private Rigidbody2D doopleRigidbody;
    private string currentAnimaton;
    private BoxCollider2D doopleBox;
    private bool isRunning;
    private bool isAttacking;
    private PolygonCollider2D PoleAttackHitbox;
    private List<Collider2D> attackedResults;
    public bool enemyHitByPole;
    public UnityEvent hitEnemy;

    [SerializeField] private Animator doopleAnimator;
    [SerializeField] private GameObject styleWheelObject;
    [SerializeField] private InputSettings inputSettings;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] float runSpeed = 1f;
    [SerializeField] private LayerMask groundLayer;
    

    //Animation States
    private string doople_walk = "Doople_Walk";
    private string doople_idle = "Doople_Idle";
    private string doople_jump = "Doople_Jump";
    private string doople_attack_anim = "Doople_Attack_Anim";

    private void Awake()
    {
        doopleRigidbody = GetComponent<Rigidbody2D>();
        doopleAnimator = GetComponent<Animator>();
        doopleBox = GetComponent<BoxCollider2D>();
        PoleAttackHitbox = GetComponent<PolygonCollider2D>();
        PoleAttackHitbox.enabled = false;
        attackedResults = new List<Collider2D>();
        enemyHitByPole = false;
    }

    private void Update()
    {

        IsGrounded();

        if (!IsGrounded() && !isAttacking)
        {
            doopleAnimator.Play(doople_jump);
        }
        if (IsGrounded() && !isRunning && !isAttacking)
        {
            doopleAnimator.Play(doople_idle);
        }
        if (IsGrounded() && isRunning && !isAttacking)
        {
            doopleAnimator.Play(doople_walk);
        }

        

        //Debug.Log(doopleAnimator.GetCurrentAnimatorStateInfo(0));
        if (isAttacking)
        {
            Physics2D.OverlapCollider(PoleAttackHitbox, new ContactFilter2D().NoFilter(), attackedResults);
            for (int i = 0; i < attackedResults.Count; i++)
            {
                if (attackedResults[i].name == "Enemy" && enemyHitByPole == false)
                {
                    enemyHitByPole = true;
                    hitEnemy.Invoke();
                }
                //Debug.Log("attacked results = " + attackedResults[i]);
            }
            
        }
    }


    public void Attack(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            doopleAnimator.Play(doople_attack_anim);
            isAttacking = true;
            
        }
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }

    public void PoleAttackHitboxStart()
    {
        PoleAttackHitbox.enabled = true;
    }

    public void PoleAttackHitboxEnd()
    {
        PoleAttackHitbox.enabled = false;
        enemyHitByPole = false;
    }



    public void Run(InputAction.CallbackContext context)
    {

        float direction = 1f;

        //Debug.Log(context);
        if (context.performed && !isAttacking)
        {
            if (context.ReadValue<Vector2>().x > 0)
            {
                direction = 1f;
            } else if (context.ReadValue<Vector2>().x < 0)
            {
                direction = -1f;
            }

            doopleRigidbody.velocity = new Vector2(context.ReadValue<Vector2>().x * runSpeed, doopleRigidbody.velocity.y);
            transform.localScale = new Vector2(direction, 1);
            if (IsGrounded())
            {
            ChangeAnimationState(doople_walk);
            isRunning = true;
            }


        } else
        {
            doopleRigidbody.velocity = new Vector2(0, doopleRigidbody.velocity.y);
            if (IsGrounded())
            {
            ChangeAnimationState(doople_idle);
            }
            isRunning = false;
            //maybe put this into the updater. you can make him slide indefinitely when he is pushed because this is never called to stop him.
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded() && !isAttacking) { 
            //Debug.Log("Jump!");
            //Debug.Log("jumpForce is: " + jumpForce);
            //Debug.Log("context:" + context.phase);
            doopleRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            ChangeAnimationState(doople_jump);
        }
    }

    public void StyleWheel(InputAction.CallbackContext context)
    {


        float pointX = context.ReadValue<Vector2>().x;
        float pointY = context.ReadValue<Vector2>().y;

        float disFromZero = Mathf.Sqrt(Mathf.Pow(pointX, 2) + Mathf.Pow(pointY, 2));
        //Debug.Log(disFromZero);
        
        if (disFromZero > inputSettings.defaultDeadzoneMin && styleWheelObject.activeInHierarchy.Equals(false))
        {
            styleWheelObject.SetActive(true);

        }

        if (disFromZero < inputSettings.defaultDeadzoneMin && styleWheelObject.activeInHierarchy.Equals(true))
        {
            styleWheelObject.SetActive(false);

        }

        if (pointX < 0 && pointY < -pointX && pointY > pointX)
        {
            Debug.Log("Went into left side");
            styleWheelObject.GetComponent<Animator>().Play("StyleWheelLeft");
        }
        if (pointX > 0 && pointY < pointX && pointY > -pointX)
        {
            Debug.Log("Went into right side");
            styleWheelObject.GetComponent<Animator>().Play("StyleWheelRight");
        }
        if (pointY < 0 && pointX < -pointY && pointX > pointY)
        {
            Debug.Log("Went into down side");
            styleWheelObject.GetComponent<Animator>().Play("StyleWheelDown");
        }
        if (pointY > 0 && pointX < pointY && pointX > -pointY)
        {
            Debug.Log("Went into up side");
            styleWheelObject.GetComponent<Animator>().Play("StyleWheelUp");
        }
        //Debug.Log(context);
    }

    private bool IsGrounded()
    {
        float extraHeight = .1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(doopleBox.bounds.center, doopleBox.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        /*Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(doopleBox.bounds.center + new Vector3(doopleBox.bounds.extents.x, 0), Vector2.down * (doopleBox.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(doopleBox.bounds.center - new Vector3(doopleBox.bounds.extents.x, 0), Vector2.down * (doopleBox.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(doopleBox.bounds.center - new Vector3(doopleBox.bounds.extents.x, doopleBox.bounds.extents.y + extraHeight), Vector2.right * (doopleBox.bounds.extents.x * 2), rayColor);
        Debug.Log(raycastHit.collider);*/
        return raycastHit.collider != null;
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        doopleAnimator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}
