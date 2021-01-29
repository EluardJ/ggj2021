using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChunk : MonoBehaviour
{
    #region Variables
    public Vector2 gridPosition = default;
    #endregion

    #region Functions
    public void MoveToNewPosition(Vector2 newPosition)
    {
        gridPosition = newPosition;
    }
    #endregion
}
