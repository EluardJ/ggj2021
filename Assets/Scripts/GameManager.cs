using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public float gameTime = 60;

    public GridGenerator _gridGenerator;
    public RequestManager _requestManager;

    bool gameIsOn = false;
    [HideInInspector] public float timer = 0;

    public delegate void Function();
    public Function GameEnd;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            StartGame();

        if (gameIsOn)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
                EndGame();
        }
    }

    public void StartGame()
    {
        gameIsOn = true;

        if (_gridGenerator != null)
        {
            _gridGenerator._useAutoGeneration = false;
            _gridGenerator.SetGridLevel(_gridGenerator.GetComponent<GridLevel>());
            _gridGenerator.Generate();
        }
        if (_requestManager != null)
        {
            _requestManager._gridLevel = _gridGenerator.GetComponent<GridLevel>();
            _requestManager.InitializeRequestItems();
            _requestManager.SetRequestCount(3);
        }

        timer = gameTime;
    }

    public void EndGame()
    {
        gameIsOn = false;

        GameEnd?.Invoke();
    }
}