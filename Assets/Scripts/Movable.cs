using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    #region Unity Callbacks
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
    #endregion
}