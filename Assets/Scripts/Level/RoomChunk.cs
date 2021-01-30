﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomChunk : MonoBehaviour
{
    #region Variables
    public GridLevel level = default;
    public Vector2 gridCoords = default;
    public Item[] _items;
    #endregion

    #region Functions
    public void MoveToNewPosition(Vector2 newCoords, float chunkSize)
    {
        gridCoords = newCoords;

        transform.DOMove(new Vector3(newCoords.x, 0, newCoords.y) * chunkSize, 1.5f).OnComplete(OnMoveComplete);
    }

    public Item [] GetItems () {
        return _items;
    }

    private void OnMoveComplete()
    {
        level.RegisterMove(this, gridCoords);

        CheckIfChunkIsOutside();
    }

    private void CheckIfChunkIsOutside()
    {
        if (gridCoords.x < 0 || gridCoords.x >= level.gridDimensions
            || gridCoords.y < 0 || gridCoords.y >= level.gridDimensions)
        {
            level.RemoveChunk(gridCoords);
            Destroy(gameObject);
        }
    }
    #endregion
    [ContextMenu("Auto_Fill_Items")]
    public void Auto_Fill_Items () {
        List<Item> items = new List<Item>();
        foreach(Transform t in transform) {
            Item i = t.GetComponent<Item>();
            if (i  != null) {
                items.Add(i);
            }
        }
        _items = items.ToArray();
    }
}
