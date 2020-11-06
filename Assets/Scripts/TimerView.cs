using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [Header("Progress Bar")]
    [SerializeField] LinearProgressBar progressBar;

    [Header("Pause Play")]
    [SerializeField] Image pausePlayImage;
    [SerializeField] Sprite pauseSprite, playSprite;

    [Header("Time Texts")]
    [SerializeField] Text minutesText;
    [SerializeField] Text secondsText;
    [SerializeField] Text millisecondsText;

    void Start()
    {
        progressBar.max = ScheduleManager.instance.maxTime;
    }

    void Update()
    {
        progressBar.current = ScheduleManager.instance.timer;

        string minutes, seconds, milliseconds;
        FloatToTime(ScheduleManager.instance.timer, out minutes, out seconds, out milliseconds);
        millisecondsText.text = minutes;
        secondsText.text = seconds;
        millisecondsText.text = milliseconds;

        if (GameManager.instance.currentState == GameState.Paused) pausePlayImage.sprite = playSprite;
        else pausePlayImage.sprite = pauseSprite;
    }

    void FloatToTime(float time, out string minutes, out string seconds, out string milliseconds)
    {
        minutes = Mathf.FloorToInt(time / 60).ToString("00");
        seconds = Mathf.FloorToInt(time % 60).ToString("00");
        milliseconds = ((time - Mathf.Floor(time)) * 60).ToString("00");
    }
}
