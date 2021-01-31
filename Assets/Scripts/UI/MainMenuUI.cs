using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel = default;
    [SerializeField] GameObject tutoPanel = default;
    [SerializeField] Button startGameButton = default;

    [SerializeField] GameManager gameManager = default;

    public void StartGame()
    {
        StartCoroutine(StartGameIE());
    }

    private IEnumerator StartGameIE()
    {
        startGameButton.interactable = false;

        mainMenuPanel.SetActive(false);

        yield return new WaitUntil(() => Input.GetButton("Fire1"));

        tutoPanel.SetActive(false);

        gameManager.StartGame();
    }
}
