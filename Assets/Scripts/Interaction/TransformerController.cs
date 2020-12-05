using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerController : Breakable
{
    [SerializeField] LinearProgressBar progressBar;
    [SerializeField] float fallout = 10;

    [SerializeField] int maxHitPoint = 10;
    [SerializeField] int repairHitPoint = 1;
    [SerializeField] float repairDelay = 0.1f;

    [SerializeField] int hitPoint;
    float timer = 0;

    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        progressBar.max = maxHitPoint;
        hitPoint = maxHitPoint;
    }

    void OnEnable()
    {
        trapActivateHandler = EventBus.Subscribe<TrapActivateEvent>(OnTrapActivated);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(trapActivateHandler);
    }

    protected override void Update()
    {
        base.Update();

        progressBar.current = progressBar.current.Fallout(hitPoint, fallout);
        timer += Time.deltaTime;
    }

    public override void Break()
    {
        base.Break();
        hitPoint = 0;
    }

    public override void Repair()
    {
        if (timer < repairDelay) return;

        if (hitPoint < maxHitPoint) hitPoint += repairHitPoint;
        else
        {
            hitPoint = maxHitPoint;
            base.Repair();
        }
    }

    void OnTrapActivated(TrapActivateEvent @event)
    {
        if (@event.trap is DistractionTrap)
        {
            var distraction = @event.trap as DistractionTrap;
            var distance = Vector3.Distance(transform.position, distraction.transform.position);
            if (distance < 2) Break();
        }
    }
}
