using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridLevel))]
public class GridGenerator : MonoBehaviour
{
    #region Variables
    public int gridDimensions = 3;
    [SerializeField] GameObject[] chunkPrefabs = default;
    [SerializeField] float chunkSize = 10;

    GridLevel gridLevel = default;
    #endregion

    #region UnityCallbacks
    private void Awake()
    {
        gridLevel = GetComponent<GridLevel>();
    }

    private void Start()
    {
        GenerateGrid(gridDimensions, chunkSize);
    }
    #endregion

    #region Functions
    private void GenerateGrid(int gridDimensions, float chunkSize)
    {
        gridLevel.gridDimensions = gridDimensions;
        gridLevel.chunkSize = chunkSize;
        gridLevel.chunkPrefabs = chunkPrefabs;

        for (int i = 0; i < gridDimensions; i++)
        {
            for (int j = 0; j < gridDimensions; j++)
            {
                Vector3 position = new Vector3(chunkSize * i, 0, chunkSize * j);
                GameObject go = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)], position, Quaternion.identity);
                RoomChunk chunk = go.GetComponent<RoomChunk>();

                Vector2 gridCoords = new Vector2(i, j);
                chunk.level = gridLevel;
                chunk.gridCoords = gridCoords;

                gridLevel.chunks.Add(gridCoords, chunk);
            }
        }
    }
    #endregion
}
