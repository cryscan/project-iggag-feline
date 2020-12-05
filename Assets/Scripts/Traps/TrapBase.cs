using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivateEvent : IEvent
{
    public TrapBase trap;
    public GameObject activated;

    public TrapActivateEvent(TrapBase trap, GameObject activated)
    {
        this.trap = trap;
        this.activated = activated;
    }
}

public abstract class TrapBase : MonoBehaviour
{
    [SerializeField] GameObject activatedPrefab;

    Subscription<ScheduleTimerEvent> handler;

    void OnEnable()
    {
        handler = EventBus.Subscribe<ScheduleTimerEvent>(OnScheduleTimer);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    public virtual void Activate()
    {
        // EventBus.Publish(new TrapActivateEvent() { trap = this });
        var activated = Instantiate(activatedPrefab, transform.position, transform.rotation);
        EventBus.Publish(new TrapActivateEvent(this, activated));
        Destroy(gameObject);
    }

    public virtual float GetRange() => 0;

    void OnScheduleTimer(ScheduleTimerEvent @event)
    {
        if (@event._object == gameObject) Activate();
    }
}
