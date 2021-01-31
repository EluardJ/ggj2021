using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowIndicator : MonoBehaviour
{
    [SerializeField] Transform player = default;
    [SerializeField] Hook hook = default;
    [SerializeField] Thrower thrower = default;
    [SerializeField] Grapple grapple = default;
    [SerializeField] Transform arrow = default;
    [SerializeField] GameObject arrowBG = default;

    private void Update()
    {
        if (grapple.state != Grapple.State.Grabbing)
        {
            arrow.localScale = Vector3.zero;
            arrowBG.SetActive(false);
        }
        else
        {
            Vector3 newPos = hook.grabbedObject.transform.position;
            newPos.y = 0.2f;
            transform.position = newPos;

            Vector3 direction = thrower.GetLookDirection();

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            Vector3 size = new Vector3(1, 1, thrower.GetNormalizedCharge());
            arrow.localScale = size;
            arrowBG.SetActive(true);
        }
    }
}
