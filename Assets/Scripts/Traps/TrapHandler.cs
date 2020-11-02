using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    Mark,
    Frozen,
    Distraction
}

public class TrapEvent
{
    public GameObject trap;
    public TrapType type;
    public object data;

    public TrapEvent(GameObject target, TrapType type, object data)
    {
        this.trap = target;
        this.type = type;
        this.data = data;
    }
}

public class TrapHandler : MonoBehaviour
{
    [System.Serializable]
    public struct FrozenData
    {
        public float range;
        public float duration;
    }

    [SerializeField] TrapType type;
    [SerializeField] FrozenData frozenData;

    Subscription<ScheduleTimerEvent> scheduleHandler;

    public void Activate()
    {
        object data = null;
        if (type == TrapType.Frozen) data = frozenData;

        EventBus.Publish(new TrapEvent(gameObject, type, data));
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        scheduleHandler = EventBus.Subscribe<ScheduleTimerEvent>(OnScheduleTimer);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(scheduleHandler);
    }

    void OnScheduleTimer(ScheduleTimerEvent @event)
    {
        if (@event.prefab == gameObject) Activate();
    }
}
