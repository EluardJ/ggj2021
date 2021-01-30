using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grapple))]
public class Thrower : MonoBehaviour
{
    [SerializeField] float throwPower = 1;
    [SerializeField] float maxChargeTime = 1;

    Grapple grapple = default;

    bool isCharging = false;
    float charge = 0;

    private void Awake()
    {
        grapple = GetComponent<Grapple>();
    }

    private void Update()
    {
        if (grapple.isGrabbing)
        {
            if (Input.GetButtonDown("Fire3") && !isCharging)
            {
                isCharging = true;
            }

            if (Input.GetButton("Fire3") && isCharging)
            {
                Charge();
            }

            if (Input.GetButtonUp("Fire3") && isCharging)
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

        Debug.Log("charging : " + charge);
    }

    private void Throw()
    {
        float chargeAmount = charge / maxChargeTime;

        grapple.Throw(throwPower * chargeAmount);

        isCharging = false;

        Debug.Log("throw : " + throwPower * chargeAmount);
    }
}
