using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
 
    [SerializeField] private Camera playerCamera;

    // Update is called once per frame
    void Update()
    {
        //playerCamera.transform.position.x;
        //transform.position.Set(playerCamera.transform.position.x, playerCamera.transform.position.y, 0f);
        transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, 0f);

    }
}
