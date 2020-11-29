using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;

namespace Feline.AI.Sensors
{
    [RequireComponent(typeof(BehaviorTree))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class GuardFrozenSensor : ReGoapSensor<string, object>
    {
        [SerializeField] GameObject effect;
        [SerializeField] Light _light;

        public bool frozen { get; private set; }

        Collider _collider;
        ConeDetection detection;
        GuardAlertSensor sensor;
        BehaviorTree behavior;
        NavMeshAgent agent;

        bool stopped = false;

        Subscription<TrapActivateEvent> trapActivateHandler;

        void Awake()
        {
            _collider = GetComponent<Collider>();
            detection = GetComponent<ConeDetection>();
            sensor = GetComponent<GuardAlertSensor>();
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
            _light.enabled = false;
            _collider.enabled = false;
            detection.enabled = false;
            sensor.enabled = false;

            behavior.DisableBehavior(true);

            if (!frozen) stopped = agent.isStopped;
            agent.isStopped = true;

            frozen = true;

            yield return new WaitForSeconds(duration);

            frozen = false;

            _light.enabled = true;
            _collider.enabled = true;
            detection.enabled = true;
            sensor.enabled = true;

            behavior.EnableBehavior();
            agent.isStopped = stopped;
        }
    }

}