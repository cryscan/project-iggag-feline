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

    Collider _collider;
    ConeDetection detection;
    BehaviorTree behavior;
    NavMeshAgent agent;

    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        _collider = GetComponent<Collider>();
        detection = GetComponent<ConeDetection>();
        behavior = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
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
            var frozen = @event.trap as FrozenTrap;
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
        _collider.enabled = false;
        detection.enabled = false;

        behavior.DisableBehavior(true);

        var stopped = agent.isStopped;
        agent.isStopped = true;

        yield return new WaitForSeconds(duration);

        frozen = false;

        _light.enabled = true;
        _collider.enabled = true;
        detection.enabled = true;

        behavior.EnableBehavior();
        agent.isStopped = stopped;
    }
}
