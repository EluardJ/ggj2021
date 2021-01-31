using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookTrigger : MonoBehaviour
{
    [SerializeField] Grapple grapple = default;

    Hook hook = default;

    private void Awake()
    {
        hook = GetComponentInParent<Hook>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (grapple.state == Grapple.State.Launching && other.CompareTag("Movable"))
            hook.Grab(other.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);

        if (collision.gameObject.CompareTag("Movable"))
            hook.Grab(collision.transform);
    }
}
