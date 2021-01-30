using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour, IController
{
    public float speed = 40.0f;  
    public float cooldown = 0.35f;
    public float decay = 0.1f;
    float horizontal;
    float vertical;
    Rigidbody body;
    bool dashWasPressed;
    float consumeTime = -1f;
    Vector3 deshVelocity;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    public void UpdateController()
    {
        deshVelocity = Vector3.MoveTowards(deshVelocity, Vector3.zero, decay);
        horizontal = Input.GetAxisRaw("LookHorizontal");
        vertical = Input.GetAxisRaw("LookVertical"); 
        Vector3 look = new Vector3(horizontal, 0, vertical);
        dashWasPressed = dashWasPressed || Input.GetButtonDown("Fire1");
        if (Time.time < consumeTime + cooldown) 
        {
            dashWasPressed = false;
        }
        if (dashWasPressed) 
        {
            Vector3 dashDirection = transform.forward;
            deshVelocity = dashDirection * speed;
            dashWasPressed = false;
            consumeTime = Time.time;
        }
    }
    public Vector3 GetDashVelocity () {
        return deshVelocity;
    }
    public void FixedUpdateController()
    {  
    }
}
