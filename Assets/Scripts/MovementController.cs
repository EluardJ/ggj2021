using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IController
{
    public float runSpeed = 20.0f;  
    public float maxRotationDelta = 1f;
    Rigidbody body;
    float horizontal;
    float vertical;
    Vector3 inputDirection;
    Vector3 direction;
    float dot;
    DashController dashController;
    public bool isDrifting;
    public float _driftThreshold = 0.7f;
    public ParticleSystem[] _driftEffects;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>(); 
        dashController = GetComponent<DashController>();
    }

    // Update is called once per frame
    public void UpdateController()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical"); 

        inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        direction = Vector3.RotateTowards(transform.forward, inputDirection, maxRotationDelta * Time.deltaTime, 0.1f * Time.deltaTime);
        transform.LookAt(this.transform.position + direction, Vector3.up);
        dot = Vector3.Dot(transform.forward, inputDirection);

        bool wasDrifting = isDrifting;
        isDrifting = Vector3.Dot(body.velocity, inputDirection) < _driftThreshold;
        if(wasDrifting && !isDrifting) {
            //exit drift
            foreach (ParticleSystem effect in _driftEffects) {
                effect.Stop();
            }
        }
        else if (!wasDrifting && isDrifting) {
            //enter drift
            foreach (ParticleSystem effect in _driftEffects) {
                effect.Play();
            }
        }
    }
    public void FixedUpdateController()
    {  
        Vector3 dashVelocity = Vector3.zero;
        if (dashController != null) 
        {
            dashVelocity = dashController.GetDashVelocity();
        }
        body.AddForce(dashVelocity + transform.forward * Mathf.Clamp01(0.5f + Mathf.Clamp01(dot * 2f)) * (runSpeed * inputDirection.magnitude), ForceMode.Acceleration);
    }
}
