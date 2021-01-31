using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayCanvas : MonoBehaviour
{
    [SerializeField] GameObject panel = default;

    [SerializeField] GameManager gameManager = default;

    private void OnEnable()
    {
        gameManager.GameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        gameManager.GameEnd -= OnGameEnd;
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    private void OnGameEnd()
    {
        panel.SetActive(true);
    }
}
