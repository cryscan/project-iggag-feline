using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MemoController : MonoBehaviour
{
    struct Icon
    {
        public GameObject progress; // Icon shows up on progress bar
        public GameObject level;    // Icon shows up in the level
    }

    [SerializeField] LinearProgressBar progressBar;

    Camera _camera;
    Dictionary<ScheduleTimerEvent, Icon> icons;

    Subscription<ScheduleAddEvent> scheduleAddhandler;
    Subscription<ScheduleTimerEvent> scheduleTimerHandler;

    void Awake()
    {
        _camera = Camera.main;
    }

    void OnEnable()
    {
        scheduleAddhandler = EventBus.Subscribe<ScheduleAddEvent>(OnScheduleAdded);
        scheduleTimerHandler = EventBus.Subscribe<ScheduleTimerEvent>(OnScheduleTimer);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(scheduleAddhandler);
        EventBus.Unsubscribe(scheduleTimerHandler);
    }

    void Update()
    {
        if (GameManager.instance.currentState != GameState.Play) return;

        foreach (var pair in icons)
        {
            var position = _camera.WorldToScreenPoint(pair.Key.prefab.transform.position);
            pair.Value.level.transform.position = position;
        }
    }

    void OnScheduleAdded(ScheduleAddEvent @event)
    {
        var icon = @event.@event.prefab.GetComponent<TrapIcon>();
        if (icon)
        {
            var position = Vector3.Lerp(progressBar.startPoint, progressBar.endPoint, progressBar.amount);
            var progress = Instantiate(icon.prefab, position, Quaternion.identity);
            var level = Instantiate(icon.prefab);
            icons.Add(@event.@event, new Icon() { progress = progress, level = level });
        }
    }

    void OnScheduleTimer(ScheduleTimerEvent @event)
    {
        Destroy(icons[@event].level);
        Destroy(icons[@event].progress);

        icons.Remove(@event);
    }
}
