using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using DG.Tweening;

public class Grapple : MonoBehaviour
{
    #region Variables
    public float speed = 1;
    public float distance = 1;

    [SerializeField] Transform hook = default;
    [SerializeField] Rigidbody hookRB = default;
    [SerializeField] Transform player = default;

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
        if (Input.GetKeyDown(KeyCode.L))
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
        hookRB.isKinematic = true;

        float elapsedDistance = 0;
        Vector3 origin = hook.position;
        Vector3 direction = player.forward;

        Vector3 lastPosition = origin;

        while (elapsedDistance < distance)
        {
            Vector3 newPos = hook.position + direction * speed * Time.deltaTime;
            hookRB.MovePosition(newPos);

            float length = (lastPosition - hook.position).magnitude;
            lastPosition = hook.position;

            elapsedDistance += length;

            cursor.ChangeLength(rope.restLength + length);

            yield return null;
        }

        hookRB.isKinematic = false;

        StartCoroutine(RetrieveGrappleIE());
    }

    private IEnumerator RetrieveGrappleIE()
    {
        while (rope.restLength > 0.4f)
        {
            cursor.ChangeLength(rope.restLength - 5 * Time.deltaTime);

            yield return null;
        }
    }
    #endregion
}