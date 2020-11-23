using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Actions
{
    [RequireComponent(typeof(BehaviorTree))]
    public class GoToAction : ReGoapAction<string, object>
    {
        [UnityEngine.Tooltip("Should be Move behavior")]
        [SerializeField] ExternalBehaviorTree external;
        [SerializeField] bool interruptSpotted = true;
        [SerializeField] float speed = 4;

        BehaviorTree behavior;
        Vector3? destination;

        Subscription<GuardSpotEvent> handler;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();
            effects.Set("At Position", default(Vector3));
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

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            var position = GetGoalPosition(stackData.goalState);
            if (position.HasValue) effects.Set("At Position", position);
            else effects.Set("At Position", default(Vector3));

            return base.GetEffects(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            if (stackData.goalState.HasKey("At Position"))
                settings.Set("Objective Position", stackData.goalState.Get("At Position"));
            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey("Objective Position"))
            {
                destination = settings.Get("Objective Position") as Vector3?;
                if (destination.HasValue)
                {
                    behavior.ExternalBehavior = external;

                    behavior.SetVariableValue("Destination", destination.Value);
                    behavior.SetVariableValue("Speed", speed);
                }
                else fail(this);
            }
            else fail(this);
        }

        Vector3? GetGoalPosition(ReGoapState<string, object> state)
        {
            Vector3? result = null;
            if (state != null) result = state.Get("At Position") as Vector3?;
            return result;
        }

        void OnBehaviorEnded(Behavior behavior)
        {
            Debug.Log("[Go To] behavior ended");
            if (behavior.ExternalBehavior != external) return;
            if (behavior.ExecutionStatus == TaskStatus.Success) doneCallback(this);
            else if (behavior.ExecutionStatus == TaskStatus.Failure) failCallback(this);
        }

        void OnGuardSpotted(GuardSpotEvent @event)
        {
            if (interruptSpotted && @event.subject == gameObject)
            {
                Debug.Log("[Go To] failed on spotted");
                failCallback(this);
            }
        }
    }
}
