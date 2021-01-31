using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject panel = default;

    [SerializeField] GameManager gameManager = default;

    public void StartGame()
    {
        gameManager.StartGame();

        panel.SetActive(false);
    }
}
