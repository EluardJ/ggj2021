using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour
{
    #region Variables
    [SerializeField] float autoAddChunkInterval = 3f;
    [Range(0.1f, 2.5f)]
    public float chunkMoveTime = 1.5f;
    public Texture2D[] groundLetterTextures = default;
    /*[HideInInspector]*/ public float speedTestModifier = 1f;

    public Dictionary<Vector2, RoomChunk> chunks = new Dictionary<Vector2, RoomChunk>();
    public Dictionary<Vector2, Wall> walls = new Dictionary<Vector2, Wall>();
    [HideInInspector] public int gridDimensions = 3;
    [HideInInspector] public float chunkSize = 10f;
    [HideInInspector] public GameObject[] chunkPrefabs = default;
    [HideInInspector] public GameObject comptoirPrefab = default;
    [HideInInspector] public Vector2 comptoirCoordinates = default;
    public RequestManager _requestManager;

    float elapedTimeSinceChange = 0f;
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        elapedTimeSinceChange += Time.deltaTime;

        if (elapedTimeSinceChange > autoAddChunkInterval * (1 / speedTestModifier))
        {
            elapedTimeSinceChange = 0;
            AddChunkAtRandom();
            if (_requestManager != null)
            {
                _requestManager.OnChunckChanged();
            }
        }
    }
    #endregion

    #region Functions
    private void AddChunkAtRandom()
    {
        bool horizontal = true;
        bool positive = false;
        bool replaceComptoir = false;
        int letterID = 0;
        Vector2 spawnPosition = GetRandomSpawnPosition(ref horizontal);

        if (horizontal)
        {
            positive = spawnPosition.x > 0 ? true : false;

            if (positive)
            {
                if (comptoirCoordinates == new Vector2(0, spawnPosition.y))
                    replaceComptoir = true;

                letterID = chunks[new Vector2(0, spawnPosition.y)].letterID;
            }
            else
            {
                if (comptoirCoordinates == new Vector2(gridDimensions - 1, spawnPosition.y))
                    replaceComptoir = true;

                letterID = chunks[new Vector2(gridDimensions - 1, spawnPosition.y)].letterID;
            }
        }
        else
        {
            positive = spawnPosition.y > 0 ? true : false;

            if (positive)
            {
                if (comptoirCoordinates == new Vector2(spawnPosition.x, 0))
                    replaceComptoir = true;

                letterID = chunks[new Vector2(spawnPosition.x, 0)].letterID;
            }
            else
            {
                if (comptoirCoordinates == new Vector2(spawnPosition.x, gridDimensions - 1))
                    replaceComptoir = true;

                letterID = chunks[new Vector2(spawnPosition.x, 0)].letterID;
            }
        }

        RoomChunk chunk = default;
        if (replaceComptoir)
            chunk = ReplaceComptoir(spawnPosition);
        else
            chunk = GetNewChunk(spawnPosition);

        chunk.SetLetterID(letterID);

        MoveRow(chunk, horizontal, positive);
    }

    private RoomChunk GetNewChunk(Vector2 spawnPosition)
    {
        GameObject go = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)], new Vector3(spawnPosition.x * chunkSize, 0, spawnPosition.y * chunkSize), Quaternion.identity);
        RoomChunk chunk = go.GetComponent<RoomChunk>();
        chunk.level = this;
        chunk.gridCoords = spawnPosition;

        return chunk;
    }

    private RoomChunk ReplaceComptoir(Vector2 spawnPosition)
    {
        GameObject go = Instantiate(comptoirPrefab, new Vector3(spawnPosition.x * chunkSize, 0, spawnPosition.y * chunkSize), Quaternion.identity);
        RoomChunk chunk = go.GetComponent<RoomChunk>();
        chunk.level = this;
        chunk.gridCoords = spawnPosition;

        chunks[comptoirCoordinates].comptoir = false;
        chunk.comptoir = true;

        return chunk;
    }

    private void MoveRow(RoomChunk newChunk, bool horizontal, bool positive)
    {
        if (horizontal)
        {
            for (int i = 0; i < gridDimensions; i++)
            {
                RoomChunk chunk = chunks[new Vector2(i, newChunk.gridCoords.y)];
                Vector2 nextCoords = positive ? chunk.gridCoords + Vector2.left : chunk.gridCoords + Vector2.right;
                chunk.MoveToNewPosition(nextCoords, chunkSize);

                if (positive)
                {
                    if (i == 0)
                        walls[nextCoords].Open();
                }
                else
                {
                    if (i == gridDimensions - 1)
                        walls[nextCoords].Open();
                }
            }

            Vector2 newCoords = positive ? newChunk.gridCoords + Vector2.left : newChunk.gridCoords + Vector2.right;

            walls[newChunk.gridCoords].Open();
            newChunk.MoveToNewPosition(newCoords, chunkSize);
        }
        else
        {
            for (int i = 0; i < gridDimensions; i++)
            {
                RoomChunk chunk = chunks[new Vector2(newChunk.gridCoords.x, i)];
                Vector2 nextCoords = positive ? chunk.gridCoords + Vector2.down : chunk.gridCoords + Vector2.up;
                chunk.MoveToNewPosition(nextCoords, chunkSize);

                if (positive)
                {
                    if (i == 0)
                        walls[nextCoords].Open();
                }
                else
                {
                    if (i == gridDimensions - 1)
                        walls[nextCoords].Open();
                }
            }

            Vector2 newCoords = positive ? newChunk.gridCoords + Vector2.down : newChunk.gridCoords + Vector2.up;

            walls[newChunk.gridCoords].Open();
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
