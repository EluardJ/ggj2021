using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using DG.Tweening;

public class Grapple : MonoBehaviour
{
    #region Variables
    public float speed = 1;
    public float comeBackSpeed = 1;
    public float distance = 1;
    public float minRopeLength = 0.2f;

    [SerializeField] Transform hook = default;
    [SerializeField] Rigidbody hookRB = default;
    [SerializeField] Collider hookCollider = default;
    [SerializeField] Transform player = default;
    [SerializeField] Transform hookHolder = default;

    ObiRopeCursor cursor;
    ObiRope rope;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        cursor = GetComponent<ObiRopeCursor>();
        rope = cursor.GetComponent<ObiRope>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) || Input.GetButtonDown("Fire3"))
            LaunchGrapple();
    }
    #endregion

    #region Functions
    private void LaunchGrapple()
    {
        StartCoroutine(LaunchGrappleIE());
    }

    private IEnumerator LaunchGrappleIE()
    {
        hook.parent = null;
        hookRB.isKinematic = true;
        hookCollider.enabled = true;

        Vector3 target = player.position + player.forward * distance;
        target.y = hookHolder.position.y;

        Vector3 lastPosition = hook.position;

        while (true)
        {
            Vector3 newPos = Vector3.MoveTowards(hook.position, target, speed * Time.fixedDeltaTime);
            hookRB.MovePosition(newPos);

            float length = Vector3.Distance(lastPosition, hook.position);
            lastPosition = hook.position;
            cursor.ChangeLength(rope.restLength + length / 2f);

            if (Vector3.Distance(hook.position, target) < 0.001f)
                break;

            yield return new WaitForFixedUpdate();
        }

        hookRB.isKinematic = false;

        StartCoroutine(RetrieveGrappleIE());
    }

    private IEnumerator RetrieveGrappleIE()
    {
        while (rope.restLength > minRopeLength)
        {
            float newLength = rope.restLength - comeBackSpeed * Time.deltaTime;
            newLength = Mathf.Max(newLength, minRopeLength);

            cursor.ChangeLength(rope.restLength - comeBackSpeed * Time.deltaTime);

            yield return null;
        }

        cursor.ChangeLength(minRopeLength);

        hookCollider.enabled = false;
        hookRB.isKinematic = true;
        //hook.DORotate(hookHolder.forward, 0.2f);
        //hook.DOMove(hookHolder.position + hookHolder.forward * 0.5f, 0.2f).OnComplete(RetrieveHook);
    }

    //private void RetrieveHook()
    //{
    //    hook.parent = hookHolder;
    //}
    #endregion
}