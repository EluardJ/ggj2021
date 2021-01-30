using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour, IController
{
    float horizontal;
    float vertical;
    public float maxRotationDelta = 2f;
    Vector3 inputDirection;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateController()
    {
        horizontal = Input.GetAxisRaw("LookHorizontal");
        vertical = Input.GetAxisRaw("LookVertical"); 
        inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        direction = Vector3.RotateTowards(transform.forward, inputDirection, maxRotationDelta * Time.deltaTime, 0.1f * Time.deltaTime);
        transform.LookAt(this.transform.position + direction, Vector3.up);
    }
    public void FixedUpdateController()
    {}
}
