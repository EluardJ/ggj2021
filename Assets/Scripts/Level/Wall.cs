using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{
    Animator animator = default;
    new Collider collider = default;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
    }

    public void Open()
    {
        collider.enabled = false;
        animator.SetTrigger("Open");

        StartCoroutine(CloseAfterDelayIE());
    }

    public IEnumerator CloseAfterDelayIE()
    {
        yield return new WaitForSeconds(1.5f);

        collider.enabled = true;
        animator.SetTrigger("Close");
    }
}