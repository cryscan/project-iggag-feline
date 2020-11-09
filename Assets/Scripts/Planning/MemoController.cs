using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MemoController : MonoBehaviour
{
    struct Icon
    {
        public GameObject onBar;
        public GameObject inGame;

        public Icon(GameObject onBar, GameObject inGame)
        {
            this.onBar = onBar;
            this.inGame = inGame;
        }
    }

    [SerializeField] LinearProgressBar progress;

    Camera _camera;
    Dictionary<ScheduleTimerEvent, Icon> icons = new Dictionary<ScheduleTimerEvent, Icon>();

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
        // if (GameManager.instance.currentState != GameState.Play) return;

        foreach (var pair in icons)
        {
            var position = _camera.WorldToScreenPoint(pair.Key.position);
            pair.Value.inGame.transform.position = position;
            // Debug.Log($"[Icon] {position}");
        }
    }

    void OnScheduleAdded(ScheduleAddEvent @event) => AddIcon(@event.@event);

    void OnScheduleTimer(ScheduleTimerEvent @event)
    {
        if (icons.ContainsKey(@event))
        {
            var icon = icons[@event];
            Destroy(icon.inGame);
            Destroy(icon.onBar);

            icons.Remove(@event);
        }
    }

    void AddIcon(ScheduleTimerEvent @event)
    {
        var icon = @event.prefab.GetComponent<TrapIcon>();
        if (icon)
        {
            var position = Vector3.Lerp(progress.start, progress.end, progress.amount);
            var onBar = Instantiate(icon.prefab, position, Quaternion.identity, transform);
            var inGame = Instantiate(icon.prefab, transform);
            icons.Add(@event, new Icon(onBar, inGame));
        }
    }
}
