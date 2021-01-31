using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ComptoirVaccum : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Movable movable = other.GetComponent<Movable>();
        if (movable != null)
            movable.PullInComptoir(transform);
    }
}
