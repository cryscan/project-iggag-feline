using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositiveTimeController : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        var time = ScheduleManager.instance.timer;
        text.text = $"Timer: {time.ToString("0.0")}";
    }
}
