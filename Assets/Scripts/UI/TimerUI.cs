using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText = default;

    [SerializeField] GameManager gameManager = default;

    private void Update()
    {
        timerText.SetText("{0}", gameManager.timer);
    }
}
