using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movable : MonoBehaviour
{
    #region Unity Callbacks
    new Collider collider = default;
    ComptoirChunk comptoir = null;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChunkZone"))
            ReparentToChunk(other.transform);
    }
    #endregion

    #region Functions
    private void ReparentToChunk(Transform movablesInChunk)
    {
        transform.parent = movablesInChunk;
    }

    public void TemporarilyDisableCollisionsWithPlayer()
    {
        StartCoroutine(TemporarilyDisableCollisionsWithPlayerIE());
    }

    private IEnumerator TemporarilyDisableCollisionsWithPlayerIE()
    {
        gameObject.layer = 11;

        yield return new WaitForSeconds(1.5f);

        gameObject.layer = 10;
    }

    public void PullInComptoir(Transform comptTrf, ComptoirChunk comptoirChunk)
    {
        if (collider != null)
            collider.enabled = false;

        comptoir = comptoirChunk;

        transform.DOMove(comptTrf.position, 0.5f);
        transform.DOScale(Vector3.zero, 1f).OnComplete(OnPulledIn);
    }

    private void OnPulledIn()
    {

    }
    #endregion
}