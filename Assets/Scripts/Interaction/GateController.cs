using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateController : Breakable
{
    [SerializeField] string[] accessTags = { "Enemy" };

    public bool open { get; private set; } = false;

    Animator animator;

    int priority = 0;

    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator) enabled = false;
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
        animator.SetBool("Open", open);
    }

    public void Open(float duration, int priority)
    {
        if (broken) return;
        if (this.priority > priority) return;

        this.priority = priority;

        StopAllCoroutines();
        StartCoroutine(OpenCoroutine(duration));
    }

    public void Close()
    {
        if (broken) return;

        priority = 0;
        open = false;
    }

    public override void Break()
    {
        base.Break();

        StopAllCoroutines();
        open = true;
    }

    void OnTrapActivated(TrapActivateEvent @event)
    {
        /*
        if (@event.trap is FrozenTrap)
        {
            FrozenTrap frozen = (FrozenTrap)@event.trap;
            var distance = Vector3.Distance(frozen.transform.position, transform.position);
            if (distance < frozen.range) Break();
        }
        */
    }

    IEnumerator OpenCoroutine(float duration)
    {
        open = true;
        yield return new WaitForSeconds(duration);
        open = false;

        priority = 0;
    }

    void OnTriggerStay(Collider collider)
    {
        if (accessTags.Contains(collider.gameObject.tag))
            Open(1.0f, 1);
    }
}
