using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameManager gameManager = default;

    public void StartGame()
    {
        gameManager.StartGame();

        gameObject.SetActive(false);
    }
}
