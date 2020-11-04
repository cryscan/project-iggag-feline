using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ScheduleTimerEvent
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

    public List<ScheduleTimerEvent> schedules = new List<ScheduleTimerEvent>();

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

        if (state == GameState.Play)
        {
            /*
            if (schedules.Count > 0 && schedules[0].timer < timer)
            {
                var schedule = schedules[0];
                EventBus.Publish(schedule);
                schedules.RemoveAt(0);
            }
            */
        }
    }

    IEnumerator ScheduleCoroutine()
    {
        while (schedules.Count > 0)
        {
            var schedule = schedules[0];
            yield return new WaitForSeconds(schedule.timer - timer);

            EventBus.Publish(schedule);
            schedules.RemoveAt(0);
        }
    }

    public void AddSchedule(GameObject prefab, Vector3 position) => schedules.Add(new ScheduleTimerEvent(timer, prefab, position));


    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.current == GameState.Play)
        {
            if (@event.previous == GameState.Plan)
            {
                timer = 0;
                StartCoroutine(ScheduleCoroutine());
            }

            schedules.Sort((x, y) => { return (int)(x.timer - y.timer); });

            // Instantiate all traps.
            foreach (var schedule in schedules)
            {
                var _object = Instantiate(schedule.prefab, schedule.position, Quaternion.identity);
                schedule.prefab = _object;
            }
        }

        if (@event.current == GameState.Plan)
        {
            schedules = new List<ScheduleTimerEvent>();
            timer = 0;
        }
    }
}
