﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduleAddEvent : IEvent { public ScheduleTimerEvent @event; }

public class ScheduleDeployEvent : IEvent { }

[System.Serializable]
public class ScheduleTimerEvent : IEvent
{
    public float timer;
    public GameObject prefab;
    public GameObject _object;
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

    public string sceneName { get; private set; }
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
    }


    void Update()
    {
        var state = GameManager.instance.currentState;
        if (state == GameState.Plan || state == GameState.Play) timer += Time.deltaTime;
    }

    public void SetMaxTime(float time) => maxTime = time;

    public void SetSceneName(string name) => sceneName = name;

    public void ResetTimer() => timer = 0;

    public void ClearSchedule() => schedules = new List<ScheduleTimerEvent>();

    public void AddSchedule(float timer, GameObject prefab, Vector3 position, bool planning)
    {
        var @event = new ScheduleTimerEvent(timer, prefab, position);
        schedules.Add(@event);
        if (planning) EventBus.Publish(new ScheduleAddEvent() { @event = @event });

        Debug.Log($"[Schedule Manager] schedule {@event} added");
    }

    public void AddSchedule(GameObject prefab, Vector3 position) => AddSchedule(timer, prefab, position, true);

    public void DeploySchedule()
    {
        schedules.Sort((x, y) => (int)(x.timer - y.timer));

        // Instantiate all traps.
        foreach (var schedule in schedules)
        {
            schedule._object = Instantiate(schedule.prefab, schedule.position, Quaternion.identity);
            Debug.Log($"[Schedule Manager] schedule {schedule} deployed");
        }

        EventBus.Publish(new ScheduleDeployEvent());
    }

    /*
    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.previous == GameState.Plan && @event.current == GameState.Play)
        {
            ResetTimer();
            DeploySchedule();
        }

        if (@event.previous != GameState.Paused && @event.current == GameState.Plan)
        {
            schedules = new List<ScheduleTimerEvent>();
            ResetTimer();
        }
    }
    */
}
