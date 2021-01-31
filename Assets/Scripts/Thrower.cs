using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grapple))]
public class Thrower : MonoBehaviour
{
    [SerializeField] float throwPower = 1;
    [Range(0, 1)]
    [SerializeField] float minimumPowerRatio = 0.1f;
    [SerializeField] float maxChargeTime = 1;
    [SerializeField] float upwardModifier = 1;

    Grapple grapple = default;

    [HideInInspector] public bool isCharging = false;
    float charge = 0;
    float previousInput = 0;

    Vector3 previousLookDirection = Vector3.zero;

    private void Awake()
    {
        grapple = GetComponent<Grapple>();
    }

    private void Update()
    {
        bool inputOn = false;
        bool input = false;
        bool inputUp = false;
        if (previousInput == 0 && Input.GetAxisRaw("RightTrigger") > 0)
            inputOn = true;
        else if (previousInput == 1 && Input.GetAxisRaw("RightTrigger") < 1)
            inputUp = true;
        else if (Input.GetAxisRaw("RightTrigger") > 0)
            input = true;

        previousInput = Input.GetAxisRaw("RightTrigger");

        if (grapple.isGrabbing)
        {
            if (inputOn && !isCharging)
            {
                isCharging = true;
            }

            if (input && isCharging)
            {
                Charge();
            }

            if (inputUp && isCharging)
            {
                Throw();
            }
        }
    }

    public void Grab()
    {
        charge = 0;
    }

    private void Charge()
    {
        charge += Time.deltaTime;

        if (charge > maxChargeTime)
            charge = maxChargeTime;
    }

    private void Throw()
    {
        float chargeAmount = charge / maxChargeTime;
        chargeAmount = Mathf.Lerp(throwPower * minimumPowerRatio, throwPower, chargeAmount);

        Vector3 direction = GetLookDirection() + Vector3.up * upwardModifier;

        grapple.Throw(chargeAmount * direction);

        isCharging = false;
    }

    public float GetNormalizedCharge()
    {
        return charge / maxChargeTime;
    }

    public Vector3 GetLookDirection()
    {
        float horizontal = Input.GetAxisRaw("LookHorizontal");
        float vertical = Input.GetAxisRaw("LookVertical");

        Vector3 output = new Vector3(horizontal, 0, vertical).normalized;

        if (output.magnitude < 0.01f)
        {
            return previousLookDirection;
            //return grapple.player.forward;
        }
        else
        {
            previousLookDirection = output;
            return output;
        }

    }
}
