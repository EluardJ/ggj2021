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
        float value = gameManager.timer;

        if (value < 0)
            value = 0;


        if (value > 99)
            timerText.SetText("{0:0}", value);
        else
            timerText.SetText("{0:2}", value);

        progress.fillAmount = (value / gameManager.gameTime);
    }
}