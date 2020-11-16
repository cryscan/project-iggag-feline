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
        public LineRenderer lineRenderer;

        public Icon(GameObject onBar, GameObject inGame, Material material)
        {
            this.onBar = onBar;
            this.inGame = inGame;

            lineRenderer = inGame.GetComponent<LineRenderer>();
            if (!lineRenderer)
                lineRenderer = inGame.AddComponent<LineRenderer>();

            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            lineRenderer.material = material;
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
        }
    }

    [SerializeField] Camera _camera;
    [SerializeField] Material material;
    [SerializeField] LinearProgressBar progress;

    Camera mainCamera;

    Dictionary<ScheduleTimerEvent, Icon> icons = new Dictionary<ScheduleTimerEvent, Icon>();

    Subscription<ScheduleAddEvent> scheduleAddhandler;
    Subscription<ScheduleTimerEvent> scheduleTimerHandler;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        foreach (var schedule in ScheduleManager.instance.schedules)
            AddIcon(schedule);
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
            var position = mainCamera.WorldToScreenPoint(pair.Key.position);
            pair.Value.inGame.transform.position = position;

            var direction = position - _camera.transform.position;
            if (Vector3.Dot(_camera.transform.forward, direction) > 0)
            {
                pair.Value.inGame.SetActive(true);

                // pair.Value.lineRenderer.SetPosition(0, _camera.ScreenToWorldPoint(pair.Value.onBar.transform.position));
                position = pair.Value.onBar.transform.position;
                position.z = 10;
                pair.Value.lineRenderer.SetPosition(0, _camera.ScreenToWorldPoint(position));

                position = pair.Value.inGame.transform.position;
                position.z = 10;
                pair.Value.lineRenderer.SetPosition(1, _camera.ScreenToWorldPoint(position));
            }
            else
                pair.Value.inGame.SetActive(false);
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
            var amount = @event.timer / progress.max;
            var position = progress.GetMiddlePoint(amount);
            Debug.Log($"{Screen.width}, {Screen.height}, {progress.start}, {progress.end}, {position.x}");

            var onBar = Instantiate(icon.prefab, position, Quaternion.identity, transform);
            var inGame = Instantiate(icon.prefab, transform);
            icons.Add(@event, new Icon(onBar, inGame, material));
        }
    }
}
