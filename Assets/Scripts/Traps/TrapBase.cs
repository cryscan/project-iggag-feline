using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivateEvent
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

    public void Activate()
    {
        // EventBus.Publish(new TrapActivateEvent() { trap = this });
        var activated = Instantiate(activatedPrefab, transform.position, transform.rotation);
        EventBus.Publish(new TrapActivateEvent(this, activated));
        Destroy(gameObject);
    }

    void OnScheduleTimer(ScheduleTimerEvent @event)
    {
        if (@event.prefab == gameObject) Activate();
    }
}
