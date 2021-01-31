using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText = default;

    [SerializeField] GameManager gameManager = default;

    [SerializeField] Image progress = default;

    private void Update()
    {
        if (gameManager.timer > 99)
            timerText.SetText("{0:0}", gameManager.timer);
        else
            timerText.SetText("{0:2}", gameManager.timer);

        progress.fillAmount = (gameManager.timer / gameManager.gameTime);
    }
}