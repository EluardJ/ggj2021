using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ComptoirVaccum : MonoBehaviour
{
    ComptoirChunk comptoir = default;

    private void Awake()
    {
        comptoir = GetComponentInParent<ComptoirChunk>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Movable movable = other.GetComponent<Movable>();
        if (movable != null)
            movable.PullInComptoir(transform, comptoir);
    }
}
