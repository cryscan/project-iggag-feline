using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ScheduleTimerEvent
{
    public float timer;
    public GameObject effector;

    public ScheduleTimerEvent(float timer, GameObject effector)
    {
        this.timer = timer;
        this.effector = effector;
    }
}

public class ScheduleManager : MonoBehaviour
{
    public static ScheduleManager instance { get; private set; }

    [SerializeField] List<ScheduleTimerEvent> schedules;

    float timer = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (GameManager.instance.currentState == GameState.Play)
        {
            timer += Time.deltaTime;
            if (schedules.Count > 0 && schedules[0].timer < timer)
            {
                var schedule = schedules[0];
                EventBus.Publish(schedule);
                schedules.RemoveAt(0);
            }
        }
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.current == GameState.Play)
            schedules.Sort((x, y) => { return (int)(x.timer - y.timer); });
        else if (@event.current == GameState.Plan)
            timer = 0;
    }
}
