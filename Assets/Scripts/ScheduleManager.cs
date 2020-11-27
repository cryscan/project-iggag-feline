using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduleAddEvent : IEvent { public ScheduleTimerEvent @event; }

[System.Serializable]
public class ScheduleTimerEvent : IEvent
{
    public float timer;
    public GameObject prefab;
    public Vector3 position;

    public ScheduleTimerEvent(float timer, GameObject prefab, Vector3 position)
    {
        this.timer = timer;
        this.prefab = prefab;
        this.position = position;
    }
}

public class ScheduleManager : MonoBehaviour
{
    public static ScheduleManager instance { get; private set; }

    public List<ScheduleTimerEvent> schedules { get; private set; } = new List<ScheduleTimerEvent>();

    public float maxTime { get; private set; } = 60;
    public float timer { get; private set; } = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }


    void Update()
    {
        var state = GameManager.instance.currentState;
        if (state == GameState.Plan || state == GameState.Play) timer += Time.deltaTime;
    }

    IEnumerator ScheduleExecuteCoroutine()
    {
        while (schedules.Count > 0)
        {
            // Important, or the first one will be missing on the timeline.
            yield return null;

            var schedule = schedules[0];
            schedules.RemoveAt(0);

            yield return new WaitForSeconds(schedule.timer - timer);
            EventBus.Publish(schedule);
        }
    }

    public void SetMaxTime(float time) => maxTime = time;

    public void AddSchedule(GameObject prefab, Vector3 position)
    {
        var @event = new ScheduleTimerEvent(timer, prefab, position);
        schedules.Add(@event);
        EventBus.Publish(new ScheduleAddEvent() { @event = @event });
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.previous == GameState.Plan && @event.current == GameState.Play)
        {
            timer = 0;

            schedules.Sort((x, y) => (int)(x.timer - y.timer));

            // Instantiate all traps.
            foreach (var schedule in schedules)
            {
                var _object = Instantiate(schedule.prefab, schedule.position, Quaternion.identity);
                schedule.prefab = _object;
            }

            StopAllCoroutines();
            StartCoroutine(ScheduleExecuteCoroutine());
        }

        if (@event.previous != GameState.Paused && @event.current == GameState.Plan)
        {
            schedules = new List<ScheduleTimerEvent>();
            timer = 0;
        }
    }
}
