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
        dashWasPressed = dashWasPressed || Input.GetButtonDown("Fire4");
        if (Time.time < consumeTime + cooldown) 
        {
            dashWasPressed = false;
        }
        if (dashWasPressed) 
        {
            deshVelocity = new Vector3(horizontal, 0, vertical) * speed;
            dashWasPressed = false;
        }
    }
    public Vector3 GetDashVelocity () {
        return deshVelocity;
    }
    public void FixedUpdateController()
    {  
        // if (dashWasPressed) {
        //     body.AddForce(new Vector3(horizontal, 0, vertical) * speed, ForceMode.Impulse);
        //     dashWasPressed = false;
        //     consumeTime = Time.time;
        // }
    }
}
