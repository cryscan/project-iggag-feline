using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Actions
{
    public class SearchAction : ReGoapAction<string, object>
    {
        [UnityEngine.Tooltip("Should be Search behavior")]
        [SerializeField] ExternalBehaviorTree external;
        [SerializeField] bool interruptSpotted = true;
        [SerializeField] float speed = 2;

        BehaviorTree behavior;

        Subscription<GuardSpotEvent> handler;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();

            preconditions.Set("Alerted", true);
            preconditions.Set("Can See Player", false);

            effects.Set("Can See Player", true);
        }

        void OnEnable()
        {
            behavior.OnBehaviorEnd += OnBehaviorEnded;
            handler = EventBus.Subscribe<GuardSpotEvent>(OnGuardSpotted);
        }

        void OnDisable()
        {
            behavior.OnBehaviorEnd -= OnBehaviorEnded;
            EventBus.Unsubscribe(handler);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var state = agent.GetMemory().GetWorldState();

            if (state.HasKey("Spotted Position"))
            {
                Vector3 position = (Vector3)state.Get("Spotted Position");
                preconditions.Set("At Position", position);
            }

            return base.GetPreconditions(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;
            behavior.SetVariableValue("Speed", speed);

            StartCoroutine(ActionCheckCoroutine());
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            base.Exit(next);
        }

        IEnumerator ActionCheckCoroutine()
        {
            var state = agent.GetMemory().GetWorldState();

            while (true)
            {
                bool alerted = (bool)state.Get("Alerted");
                bool canSeePlayer = (bool)state.Get("Can See Player");

                if (!alerted) failCallback(this);
                if (canSeePlayer) doneCallback(this);

                yield return null;
            }
        }

        void OnBehaviorEnded(Behavior behavior)
        {
            if (behavior.ExecutionStatus == TaskStatus.Success) doneCallback(this);
            else if (behavior.ExecutionStatus == TaskStatus.Failure) failCallback(this);
        }

        void OnGuardSpotted(GuardSpotEvent @event)
        {
            if (interruptSpotted && @event.subject == gameObject)
            {
                Debug.Log("[Search] failed on Alert");
                failCallback(this);
            }
        }
    }
}
