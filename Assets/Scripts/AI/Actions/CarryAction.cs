using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Core;
using ReGoap.Unity;
using System;

namespace Feline.AI.Actions
{
    public class CarryAction : ReGoapAction<string, object>
    {
        [SerializeField] ExternalBehaviorTree external;

        [SerializeField] Transform holder;
        [SerializeField] float range = 2;
        [SerializeField] float duration = 2;

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();
            base.Awake();
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var carryable = stackData.goalState.Get("Carrying") as Carryable;
            if (carryable) preconditions.Set("At Position", carryable.transform.position);

            preconditions.Set($"Reserved Role Type", "CarryRole");

            return base.GetPreconditions(stackData);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            var carryable = stackData.goalState.Get("Carrying") as Carryable;
            if (carryable) effects.Set("Carrying", carryable);
            else effects.Set("Carrying", null);

            return base.GetEffects(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var carryable = stackData.goalState.Get("Carrying") as Carryable;
            if (carryable) settings.Set("Objective Carryable", carryable);
            else settings.Set("Objective Carryable", null);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;

            var carryable = settings.Get("Objective Carryable") as Carryable;
            if (carryable != null)
            {
                var distance = Vector3.Distance(transform.position, carryable.transform.position);
                if (distance > range) fail(this);
                else
                {
                    if (carryable.Carry(holder)) StartCoroutine(ActionCheckCoroutine());
                    else fail(this);
                }
            }
            else fail(this);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            base.Exit(next);
        }

        IEnumerator ActionCheckCoroutine()
        {
            yield return new WaitForSeconds(duration);
            doneCallback(this);
        }
    }
}