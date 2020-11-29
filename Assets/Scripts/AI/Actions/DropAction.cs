using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;
using ReGoap.Core;
using System;

namespace Feline.AI.Actions
{
    public class DropAction : ReGoapAction<string, object>
    {
        [SerializeField] ExternalBehaviorTree external;

        [SerializeField] float duration = 2;

        BehaviorTree behavior;

        string type = "CarryRole";

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();
            base.Awake();

            preconditions.Set($"Reserved Role Type", type);
            effects.Set($"Has Role {type}", false);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var role = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as CarryRole;
            if (role)
            {
                preconditions.Set("Carrying", role.carryable);
                preconditions.Set("At Position", role.destination.position);
            }

            return base.GetPreconditions(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var role = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as CarryRole;
            if (role) settings.Set($"Objective {type}", role);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;

            var role = settings.Get($"Objective {type}") as CarryRole;
            if (role != null) StartCoroutine(ActionCheckCoroutine(role));
            else fail(this);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            base.Exit(next);
        }

        IEnumerator ActionCheckCoroutine(CarryRole role)
        {
            yield return new WaitForSeconds(duration);
            role.carryable.Drop();
            role.enabled = false;
            doneCallback(this);
        }
    }
}
