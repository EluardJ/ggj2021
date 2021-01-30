using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoomChunk : MonoBehaviour
{
    #region Variables
    public GridLevel level = default;
    public Vector2 gridCoords = default;
    public Transform[] _itemHandles;
    public Item[] _items;

    [HideInInspector] public bool comptoir = false;
    #endregion

    #region Functions
    public void MoveToNewPosition(Vector2 newCoords, float chunkSize)
    {
        if (comptoir)
            level.comptoirCoordinates = newCoords;

        gridCoords = newCoords;

        transform.DOMove(new Vector3(newCoords.x, 0, newCoords.y) * chunkSize, level.chunkMoveTime * (1 / level.speedTestModifier)).OnComplete(OnMoveComplete);
    }

    public Item[] GetItems()
    {
        return _items;
    }

    public Transform[] GetItemHandles()
    {
        return _itemHandles;
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
    public void Auto_Fill_Items()
    {
        List<Item> items = new List<Item>();
        foreach (Transform t in transform)
        {
            Item i = t.GetComponent<Item>();
            if (i != null)
            {
                items.Add(i);
            }
        }
        _items = items.ToArray();
    }
}
