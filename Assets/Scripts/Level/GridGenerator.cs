using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridLevel))]
public class GridGenerator : MonoBehaviour
{
    #region Variables
    public int gridDimensions = 3;
    [SerializeField] GameObject wallPrefab = default;
    [SerializeField] GameObject seeThroughwallPrefab = default;
    [SerializeField] GameObject[] chunkPrefabs = default;
    [SerializeField] GameObject comptoirPrefab = default;
    [SerializeField] float chunkSize = 10;
    public bool _useAutoGeneration = true;

    GridLevel _gridLevel = default;
    #endregion

    #region UnityCallbacks
    private void Awake()
    {
        _gridLevel = GetComponent<GridLevel>();
    }

    private void Start()
    {
        if (_useAutoGeneration)
        {
            Generate();
        }
    }
    #endregion
    public void SetGridLevel (GridLevel gridLevel) {
        _gridLevel = gridLevel;
    }
    public void Generate()
    {
        GenerateGrid(gridDimensions, chunkSize);
        GenerateWalls();
    }

    #region Functions
    private void GenerateGrid(int gridDimensions, float chunkSize)
    {
        _gridLevel.gridDimensions = gridDimensions;
        _gridLevel.chunkSize = chunkSize;
        _gridLevel.chunkPrefabs = chunkPrefabs;
        _gridLevel.comptoirPrefab = comptoirPrefab;

        int comptoirI = Random.Range(0, gridDimensions);
        int comptoirJ = Random.Range(0, gridDimensions);

        for (int i = 0; i < gridDimensions; i++)
        {
            for (int j = 0; j < gridDimensions; j++)
            {
                Vector3 position = new Vector3(chunkSize * i, 0, chunkSize * j);

                GameObject go = null;
                if (i == comptoirI && j == comptoirJ)
                {
                    go = Instantiate(comptoirPrefab, position, Quaternion.identity);
                    go.GetComponent<RoomChunk>().comptoir = true;
                    _gridLevel.comptoirCoordinates = new Vector2(i, j);
                }
                else
                {
                    go = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)], position, Quaternion.identity);
                }

                RoomChunk chunk = go.GetComponent<RoomChunk>();

                Vector2 gridCoords = new Vector2(i, j);
                chunk.level = _gridLevel;
                chunk.gridCoords = gridCoords;

                _gridLevel.chunks.Add(gridCoords, chunk);
            }
        }
    }

    private void GenerateWalls()
    {
        for (int i = 0; i < gridDimensions; i++)
        {
            Vector3 spawnPosition = new Vector3(i * chunkSize, 0, -chunkSize / 2);
            GameObject go = Instantiate(seeThroughwallPrefab, spawnPosition, Quaternion.identity);
            _gridLevel.walls.Add(new Vector2(i, -1), go.GetComponent<Wall>());
        }

        for (int i = 0; i < gridDimensions; i++)
        {
            Vector3 spawnPosition = new Vector3(i * chunkSize, 0, (gridDimensions * chunkSize) - chunkSize / 2);
            GameObject go = Instantiate(wallPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
            _gridLevel.walls.Add(new Vector2(i, gridDimensions), go.GetComponent<Wall>());
        }

        for (int i = 0; i < gridDimensions; i++)
        {
            Vector3 spawnPosition = new Vector3((gridDimensions * chunkSize) - chunkSize / 2, 0, i * chunkSize);
            GameObject go = Instantiate(wallPrefab, spawnPosition, Quaternion.Euler(0, -90, 0));
            _gridLevel.walls.Add(new Vector2(gridDimensions, i), go.GetComponent<Wall>());
        }

        for (int i = 0; i < gridDimensions; i++)
        {
            Vector3 spawnPosition = new Vector3(-chunkSize / 2, 0, i * chunkSize);
            GameObject go = Instantiate(wallPrefab, spawnPosition, Quaternion.Euler(0, 90, 0));
            _gridLevel.walls.Add(new Vector2(-1, i), go.GetComponent<Wall>());
        }
    }
    #endregion
}
