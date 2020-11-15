using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateReaction : MonoBehaviour
{
    [SerializeField] float breakTime = 5;
    [SerializeField] string[] accessTags;

    bool broken = false;
    bool detected = false;

    Animator animator;

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

    void Update()
    {
        if (broken) return;

        if (detected)
        {
            animator.SetBool("Open", true);
            detected = false;
        }
        else
            animator.SetBool("Open", false);
    }

    public void SetBroken(bool broken) => this.broken = broken;

    void OnTrapActivated(TrapActivateEvent @event)
    {
        if (@event.trap is FrozenTrap)
        {
            FrozenTrap frozen = (FrozenTrap)@event.trap;
            var distance = Vector3.Distance(frozen.transform.position, transform.position);
            if (distance < frozen.range)
            {
                StopAllCoroutines();
                StartCoroutine(BreakCoroutine());
            }
        }
    }

    IEnumerator BreakCoroutine()
    {
        broken = true;
        animator.SetBool("Open", true);
        yield return new WaitForSeconds(breakTime);

        animator.SetBool("Open", false);
        broken = false;
    }

    void OnTriggerStay(Collider collider)
    {
        if (accessTags.Any(x => collider.gameObject.CompareTag(x)))
            detected = true;
    }
}
