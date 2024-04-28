using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StyleAttacks : MonoBehaviour
{
    
    public void AttackRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    public void AttackLeft() { }
    public void AttackUp() { }
    public void AttackDown() { }
    public void AerialAttackRight() { }
    public void AerialAttackLeft() { }
    public void AerialAttackUp() { }
    public void AerialAttackDown() { }


}
