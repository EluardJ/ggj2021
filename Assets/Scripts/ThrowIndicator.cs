using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowIndicator : MonoBehaviour
{
    [SerializeField] Transform hookTrf = default;
    [SerializeField] Transform player = default;
    [SerializeField] Thrower thrower = default;
    [SerializeField] Grapple grapple = default;
    [SerializeField] Transform arrow = default;
    [SerializeField] GameObject arrowBG = default;

    private void Update()
    {
        if (!grapple.isGrabbing)
        {
            arrow.localScale = Vector3.zero;
            arrowBG.SetActive(false);
        }
        else
        {
            Vector3 newPos = hookTrf.position;
            newPos.y = 0.2f;
            transform.position = newPos;

            newPos = player.position;
            newPos.y = 0.2f;
            Vector3 direction = (newPos - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            Vector3 size = new Vector3(1, 1, thrower.GetNormalizedCharge());
            arrow.localScale = size;
            arrowBG.SetActive(true);
        }
    }
}
