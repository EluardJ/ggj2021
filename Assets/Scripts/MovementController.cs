using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float runSpeed = 20.0f;  
    public float maxRotationDelta = 1f;
    Rigidbody body;
    float horizontal;
    float vertical;
    Vector3 direction;
    float dot;
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

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        direction = Vector3.RotateTowards(transform.forward, inputDirection, maxRotationDelta * Time.deltaTime, 0.1f * Time.deltaTime);
        transform.LookAt(this.transform.position + direction, Vector3.up);
        dot = Vector3.Dot(transform.forward, inputDirection);
    }
    private void FixedUpdate()
    {  
        body.AddForce(transform.forward * Mathf.Clamp01(0.5f + Mathf.Clamp01(dot * 2f)) * runSpeed, ForceMode.Acceleration);
    }

}
