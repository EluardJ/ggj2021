using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using DG.Tweening;

[RequireComponent(typeof(Thrower))]
public class Grapple : MonoBehaviour
{
    #region Variables
    [Header("---Values---")]
    public float speed = 1;
    public float comeBackSpeed = 1;
    public float distance = 1;
    public float minRopeLength = 0.2f;

    [Header("---Refs---")]
    public Hook hook = default;
    public Transform hookTrf = default;
    public Rigidbody hookRB = default;
    public Transform player = default;
    public Transform hookHolder = default;

    Thrower thrower = default;
    ObiRopeCursor cursor;
    ObiRope rope;

    [HideInInspector] public bool isGrabbing = false;
    [HideInInspector] public bool isLaunching = false;

    float previousInput = 0;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        thrower = GetComponent<Thrower>();
        cursor = GetComponent<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
        hook.grapple = this;
    }

    void Update()
    {
        if (previousInput < 1 && Input.GetAxisRaw("RightTrigger") > 0)
        {
            if (!isGrabbing && !isLaunching)
                LaunchGrapple();
        }

        previousInput = Input.GetAxisRaw("RightTrigger");
    }
    #endregion

    #region Functions
    private void LaunchGrapple()
    {
        StartCoroutine(LaunchGrappleIE());
    }

    private IEnumerator LaunchGrappleIE()
    {
        hookTrf.parent = null;
        hookRB.isKinematic = true;
        isLaunching = true;
        hook.ToggleActivate(true);

        Vector3 target = player.position + player.forward * distance;
        target.y = hookHolder.position.y;

        Vector3 lastPosition = hookTrf.position;

        while (true)
        {
            if (isGrabbing)
                break;

            Vector3 newPos = Vector3.MoveTowards(hookTrf.position, target, speed * Time.fixedDeltaTime);
            hookRB.MovePosition(newPos);

            float length = Vector3.Distance(lastPosition, hookTrf.position);
            lastPosition = hookTrf.position;
            cursor.ChangeLength(rope.restLength + length / 2f);

            if (Vector3.Distance(hookTrf.position, target) < 0.001f)
                break;

            yield return new WaitForFixedUpdate();
        }

        hookRB.isKinematic = false;
        hook.ToggleActivate(false);
        isLaunching = false;

        StartCoroutine(RetrieveGrappleIE());
    }

    private IEnumerator RetrieveGrappleIE()
    {
        if (!hook.isHoldingSomething)
            minRopeLength = 0.5f;
        else
            minRopeLength = 1;

        while (rope.restLength > minRopeLength)
        {
            float newLength = rope.restLength - comeBackSpeed * Time.deltaTime;
            newLength = Mathf.Max(newLength, minRopeLength);

            cursor.ChangeLength(rope.restLength - comeBackSpeed * Time.deltaTime);

            yield return null;
        }

        cursor.ChangeLength(minRopeLength);

        if (!isGrabbing)
        {
            StartCoroutine(RetrieveHookIE());
        }
    }

    private IEnumerator RetrieveHookIE()
    {
        hookRB.isKinematic = true;

        float elapsedTime = 0;
        Vector3 originPosition = hookTrf.position;
        Quaternion originRotation = hookTrf.rotation;

        while (elapsedTime < 0.2f)
        {
            elapsedTime += Time.deltaTime;

            hookTrf.position = Vector3.Lerp(originPosition, hookHolder.position + hookHolder.forward * 0.5f, elapsedTime / 0.2f);
            hookTrf.rotation = Quaternion.Lerp(originRotation, Quaternion.LookRotation(player.forward), elapsedTime / 0.2f);

            yield return null;
        }

        hookTrf.rotation = Quaternion.LookRotation(player.forward);
        hookTrf.position = hookHolder.position + hookHolder.forward * 0.5f;
        hookTrf.parent = hookHolder;
    }

    public void Throw(Vector3 force)
    {
        hook.Throw(force);

        isGrabbing = false;

        StartCoroutine(RetrieveGrappleIE());
    }

    public void Drop()
    {
        isGrabbing = false;
    }
    public void Grab()
    {
        isGrabbing = true;

        thrower.Grab();
    }
    #endregion
}