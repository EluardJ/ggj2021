using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    #region Variables
    [SerializeField] int gridDimensions = 3;
    [SerializeField] float chunkSize = 10;
    [SerializeField] GameObject chunkPrefab = default;

    Dictionary<Vector2, RoomChunk> chunks = new Dictionary<Vector2, RoomChunk>();
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            CreateGrid(gridDimensions, chunkSize);

        if (Input.GetKeyDown(KeyCode.M))
            chunks[Vector2.zero].MoveToNewPosition(Vector2.left, chunkSize);

        if (Input.GetKeyDown(KeyCode.S))
            AddChunkAtRandom();
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

    private void AddChunkAtRandom()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();

        GameObject go = Instantiate(chunkPrefab, new Vector3(spawnPosition.x * chunkSize, 0, spawnPosition.y * chunkSize), Quaternion.identity);
        RoomChunk chunk = go.GetComponent<RoomChunk>();
    }

    private Vector2 GetRandomSpawnPosition()
    {
        int x = 0;
        int y = 0;

        if (Random.value > 0.5f)
        {
            x = Random.value > 0.5f ? -1 : gridDimensions;
            y = Random.Range(0, gridDimensions);
        }
        else
        {
            x = Random.Range(0, gridDimensions);
            y = Random.value > 0.5f ? -1 : gridDimensions;
        }

        return new Vector2(x, y);
    }
    #endregion
}