using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Variables
    [SerializeField] int gridDimensions = 3;
    [SerializeField] float chunksSize = 10;
    [SerializeField] GameObject chunkPrefab = default;

    Dictionary<Vector2, RoomChunk> chunks = new Dictionary<Vector2, RoomChunk>();
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            CreateGrid(gridDimensions, chunksSize);
    }
    #endregion

    #region Functions
    private void CreateGrid(int gridDimensions, float chunksSize)
    {
        for (int i = 0; i < gridDimensions; i++)
        {
            for (int j = 0; j < gridDimensions; j++)
            {
                Vector3 position = new Vector3(chunksSize * i, 0, chunksSize * j);
                GameObject go = Instantiate(chunkPrefab, position, Quaternion.identity);
                RoomChunk chunk = go.GetComponent<RoomChunk>();

                Vector2 gridPosition = new Vector2(i, j);
                chunk.gridPosition = gridPosition;

                chunks.Add(gridPosition, chunk);
            }
        }
    }
    #endregion
}