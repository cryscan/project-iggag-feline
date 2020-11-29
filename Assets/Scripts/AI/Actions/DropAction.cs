using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Unity;
using ReGoap.Core;
using System;

namespace Feline.AI.Actions
{
    public class DropAction : ReGoapAction<string, object>
    {
        string type = "CarryRole";

        protected override void Awake()
        {
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
                preconditions.Set("At Position", role.destination);
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

            var role = settings.Get($"Objective {type}") as CarryRole;
            if (role != null)
            {
                role.carryable.Drop();
                role.enabled = false;
                done(this);
            }
            else fail(this);
        }
    }
}
