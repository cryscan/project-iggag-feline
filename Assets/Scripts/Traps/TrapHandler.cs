using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    Frozen,
    Eliminate,
    Distraction
}

public class TrapEvent
{
    public GameObject target;
    public TrapType type;
    public object data;

    public TrapEvent(GameObject target, TrapType type, object data)
    {
        this.target = target;
        this.type = type;
        this.data = data;
    }
}

public class TrapHandler : MonoBehaviour
{
    [SerializeField] TrapType type;

    void Activate()
    {
        EventBus.Publish(new TrapEvent(gameObject, type, null));
    }

    void OnScheduleTimer(ScheduleTimerEvent @event)
    {
        if (@event.prefab  == gameObject) Activate();
    }
}
