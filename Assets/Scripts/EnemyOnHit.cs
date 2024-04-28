using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FirstEnemyScript : MonoBehaviour

{
    [SerializeField] float healthPoints = 10;


    private Rigidbody2D enemyBody;


    private void Awake()
    {
        enemyBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    public void WasAttacked()
    {
        enemyBody.AddForce(new Vector2(10f, 10f), ForceMode2D.Impulse);
        healthPoints -= 2;
    }




}
