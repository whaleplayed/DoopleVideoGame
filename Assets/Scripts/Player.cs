using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{

    [SerializeField] float jumpForce = 1f;

    //Animation States
    private string doople_jump = "Doople_Jump";

    public override void Update()
    {
        base.Update();
    }

    protected override void HandleRun()
    {
        
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            //Debug.Log("Jump!");
            //Debug.Log("jumpForce is: " + jumpForce);
            //Debug.Log("context:" + context.phase);
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            ChangeAnimationState(doople_jump);
        }

    }

}
