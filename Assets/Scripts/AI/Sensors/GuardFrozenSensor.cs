using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;

[RequireComponent(typeof(BehaviorTree))]
[RequireComponent(typeof(NavMeshAgent))]
public class GuardFrozenSensor : ReGoapSensor<string, object>
{
    [SerializeField] GameObject effect;
    [SerializeField] Light _light;

    bool frozen;

    BehaviorTree behavior;
    NavMeshAgent agent;
    ConeDetection detection;

    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<ConeDetection>();
    }

    void OnEnable()
    {
        EventBus.Subscribe<TrapActivateEvent>(OnTrapActivated);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(trapActivateHandler);
    }

    public override void UpdateSensor()
    {
        effect.SetActive(frozen);
    }

    void OnTrapActivated(TrapActivateEvent @event)
    {
        var position = @event.trap.transform.position;

        if (@event.trap is FrozenTrap)
        {
            FrozenTrap frozen = (FrozenTrap)@event.trap;
            var distance = Vector3.Distance(transform.position, frozen.transform.position);

            if (distance < frozen.range) Freeze(frozen.duration);
        }
    }

    void Freeze(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(FrozenCoroutine(duration));
    }

    IEnumerator FrozenCoroutine(float duration)
    {
        frozen = true;

        _light.enabled = false;
        agent.enabled = false;
        behavior.DisableBehavior(true);
        detection.enabled = false;

        yield return new WaitForSeconds(duration);

        frozen = false;

        _light.enabled = true;
        agent.enabled = true;
        behavior.EnableBehavior();
        detection.enabled = true;
    }
}
