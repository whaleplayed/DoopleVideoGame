using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]

public abstract class Character : MonoBehaviour
{
    [Header("GameObject Variables")]
    protected BoxCollider2D m_collider;
    protected Rigidbody2D rb;

    [Header("Run Variables")]
    

    [Header("Jump Variables")]
    [SerializeField] protected LayerMask groundLayer;

    [Header("Animation Variables")]
    protected Animator animator;
    protected string currentAnimation;

    public virtual void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    public virtual void Update()
    {
        
    }

    //Mechanics
    protected void Jump()
    {

    }
    protected void Run()
    {

    }
    protected void Attack() { }

    //Sub-Mechanics
    protected virtual void HandleRun()
    {
        Run();
    }

    public virtual void HandleJump()
    {

    }

    protected bool IsGrounded()
    {
        float extraHeight = .1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_collider.bounds.center, m_collider.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        return raycastHit.collider != null;
    }

    protected void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

}
