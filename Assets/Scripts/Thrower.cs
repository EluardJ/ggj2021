using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grapple))]
public class Thrower : MonoBehaviour
{
    [SerializeField] float throwPower = 1;
    [SerializeField] float maxChargeTime = 1;
    [SerializeField] float upwardModifier = 1;

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

        Vector3 playerTweakedPosition = grapple.player.position;
        playerTweakedPosition.y = grapple.hookHolder.position.y + upwardModifier;
        Debug.Log("<color=green>" + playerTweakedPosition + "</color>");
        Vector3 direction = (grapple.player.position - grapple.hookTrf.position).normalized;

        Debug.Log(direction);

        grapple.Throw((throwPower * chargeAmount) * direction);

        isCharging = false;

        Debug.Log("throw : " + throwPower * chargeAmount);
    }
}
