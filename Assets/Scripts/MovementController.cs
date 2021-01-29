using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float runSpeed = 20.0f;  
    Rigidbody body;
    float horizontal;
    float vertical;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); 
    }
    private void FixedUpdate()
    {  
        body.velocity = new Vector3(horizontal * runSpeed, 0, vertical * runSpeed);
    }

}
