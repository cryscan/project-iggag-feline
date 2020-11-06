using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoController : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    /*
    private void Update()
    {
        text.text = "Memo:";
        foreach (var schedule in ScheduleManager.instance.schedules)
        {
            text.text = text.text + $"\n{schedule.prefab.GetComponent<TrapHandler>().type}  { schedule.timer.ToString("0.0")}";
        }
    }
    */
}
