using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateReaction : MonoBehaviour
{
    [SerializeField] string[] accessTags;
    [SerializeField] float breakTime = 5;
    [SerializeField] GameObject breakEffect;

    bool broken = false;
    bool detected = false;
    bool open = false;

    Animator animator;

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

        if (broken) return;

        if (detected)
        {
            open = true;
            detected = false;
        }
        else open = false;
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
        open = true;
        breakEffect.SetActive(true);

        yield return new WaitForSeconds(breakTime);

        open = false;
        breakEffect.SetActive(false);
        broken = false;
    }

    void OnTriggerStay(Collider collider)
    {
        if (accessTags.Any(x => collider.gameObject.CompareTag(x)))
            detected = true;
    }
}
