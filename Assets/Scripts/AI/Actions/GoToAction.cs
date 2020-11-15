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

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();
            SetDefaultEffects();
        }

        void OnEnable()
        {
            behavior.OnBehaviorEnd += OnBehaviorEnded;
        }

        void OnDisable()
        {
            behavior.OnBehaviorEnd -= OnBehaviorEnded;
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            var position = GetGoalPosition(stackData.goalState);
            if (position.HasValue) effects.Set("At", position);
            else SetDefaultEffects();

            return base.GetEffects(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            if (stackData.goalState.HasKey("At"))
                settings.Set("Objective Position", stackData.goalState.Get("At"));
            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey("Objective Position"))
            {
                var destination = settings.Get("Objective Position") as Vector3?;
                if (destination.HasValue)
                {
                    // behavior.DisableBehavior();
                    behavior.ExternalBehavior = external;
                    behavior.SetVariableValue("Destination", destination.Value);
                    // behavior.EnableBehavior();
                    return;
                }
            }

            failCallback(this);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            base.Exit(next);

            var state = agent.GetMemory().GetWorldState();
            state.Set("At", effects.Get("At"));
        }

        void SetDefaultEffects() => effects.Set("At", default(Vector3));

        Vector3? GetGoalPosition(ReGoapState<string, object> state)
        {
            Vector3? result = null;
            if (state != null) result = state.Get("At") as Vector3?;
            return result;
        }

        void OnBehaviorEnded(Behavior behavior)
        {
            switch (behavior.ExecutionStatus)
            {
                case TaskStatus.Success:
                    doneCallback(this);
                    break;
                case TaskStatus.Failure:
                    failCallback(this);
                    break;
            }
        }
    }
}
