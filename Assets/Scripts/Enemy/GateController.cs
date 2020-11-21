using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] string[] access = { "Enemy" };
    [SerializeField] GameObject breakEffect;

    public bool broken { get; private set; } = false;
    public bool open { get; private set; } = false;

    Animator animator;

    int priority = 0;
    Coroutine openCoroutine = null;

    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!animator) enabled = false;

        breakEffect.SetActive(false);
    }

    void OnEnable()
    {
        trapActivateHandler = EventBus.Subscribe<TrapActivateEvent>(OnTrapActivated);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(trapActivateHandler);
    }

    void Update()
    {
        animator.SetBool("Open", open);
        breakEffect.SetActive(broken);
    }

    public void Open(float duration, int priority)
    {
        if (broken) return;
        if (this.priority > priority) return;

        this.priority = priority;

        if (openCoroutine != null) StopCoroutine(openCoroutine);
        openCoroutine = StartCoroutine(OpenCoroutine(duration));
    }

    public void Close()
    {
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
            openCoroutine = null;
        }

        priority = 0;
        open = false;
    }

    public void Break()
    {
        StopAllCoroutines();
        broken = true;
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

        openCoroutine = null;
        priority = 0;
    }

    void OnTriggerStay(Collider collider)
    {
        if (access.Contains(collider.gameObject.tag))
            Open(1.0f, 1);
    }
}
