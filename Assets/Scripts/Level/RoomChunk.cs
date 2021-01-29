using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomChunk : MonoBehaviour
{
    #region Variables
    public Vector2 gridPosition = default;
    #endregion

    #region Functions
    public void MoveToNewPosition(Vector2 newPosition, float chunkSize)
    {
        Debug.Log("move to : " + newPosition * chunkSize);
        gridPosition = newPosition;

        transform.DOMove(newPosition * chunkSize, 1.5f);
    }
    #endregion
}
