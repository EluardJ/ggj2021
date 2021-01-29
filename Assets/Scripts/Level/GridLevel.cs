using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour
{
    #region Variables
    public int gridDimensions = 3;
    [SerializeField] float chunkSize = 10;
    [SerializeField] GameObject chunkPrefab = default;

    Dictionary<Vector2, RoomChunk> chunks = new Dictionary<Vector2, RoomChunk>();
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            CreateGrid(gridDimensions, chunkSize);

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

                Vector2 gridCoords = new Vector2(i, j);
                chunk.level = this;
                chunk.gridCoords = gridCoords;

                chunks.Add(gridCoords, chunk);
            }
        }
    }

    private void AddChunkAtRandom()
    {
        bool horizontal = true;
        Vector2 spawnPosition = GetRandomSpawnPosition(ref horizontal);

        GameObject go = Instantiate(chunkPrefab, new Vector3(spawnPosition.x * chunkSize, 0, spawnPosition.y * chunkSize), Quaternion.identity);
        RoomChunk chunk = go.GetComponent<RoomChunk>();
        chunk.level = this;
        chunk.gridCoords = spawnPosition;

        MoveRow(chunk, horizontal);
    }

    private void MoveRow(RoomChunk newChunk, bool horizontal)
    {
        bool positive = false;

        if (horizontal)
        {
            positive = newChunk.gridCoords.x > 0 ? true : false;

            for (int i = 0; i < gridDimensions; i++)
            {
                RoomChunk chunk = chunks[new Vector2(i, newChunk.gridCoords.y)];
                Vector2 nextCoords = positive ? chunk.gridCoords + Vector2.left : chunk.gridCoords + Vector2.right;
                chunk.MoveToNewPosition(nextCoords, chunkSize);
            }

            Vector2 newCoords = positive ? newChunk.gridCoords + Vector2.left : newChunk.gridCoords + Vector2.right;

            newChunk.MoveToNewPosition(newCoords, chunkSize);
        }
        else
        {
            positive = newChunk.gridCoords.y > 0 ? true : false;

            for (int i = 0; i < gridDimensions; i++)
            {
                RoomChunk chunk = chunks[new Vector2(newChunk.gridCoords.x, i)];
                Vector2 nextCoords = positive ? chunk.gridCoords + Vector2.down : chunk.gridCoords + Vector2.up;
                chunk.MoveToNewPosition(nextCoords, chunkSize);
            }

            Vector2 newCoords = positive ? newChunk.gridCoords + Vector2.down : newChunk.gridCoords + Vector2.up;

            newChunk.MoveToNewPosition(newCoords, chunkSize);
        }
    }

    public void RegisterMove(RoomChunk chunk, Vector2 newCoords)
    {
        if (chunks.ContainsKey(newCoords))
            chunks[newCoords] = chunk;
        else
            chunks.Add(newCoords, chunk);
    }

    public void RemoveChunk(Vector2 coords)
    {
        chunks.Remove(coords);
    }

    private Vector2 GetRandomSpawnPosition(ref bool horizontal)
    {
        int x = 0;
        int y = 0;

        if (Random.value > 0.5f)
        {
            x = Random.value > 0.5f ? -1 : gridDimensions;
            y = Random.Range(0, gridDimensions);
            horizontal = true;
        }
        else
        {
            x = Random.Range(0, gridDimensions);
            y = Random.value > 0.5f ? -1 : gridDimensions;
            horizontal = false;
        }

        return new Vector2(x, y);
    }
    #endregion
}
