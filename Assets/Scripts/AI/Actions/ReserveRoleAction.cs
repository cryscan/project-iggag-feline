using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Actions
{
    public class ReserveRoleAction : ReGoapAction<string, object>
    {
        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            preconditions.Clear();
            var state = stackData.goalState;
            if (state.HasKey("Reserved Role"))
            {
                var type = state.Get("Reserved Role") as string;
                preconditions.Set($"Has Role {type}", true);
            }

            return base.GetPreconditions(stackData);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            effects.Clear();
            var state = stackData.goalState;
            if (state.HasKey("Reserved Role")) effects.Set("Reserved Role", state.Get("Reserved Role"));

            return base.GetEffects(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            settings.Clear();
            var state = stackData.goalState;

            string type = null;
            if (state.HasKey("Reserved Role")) type = state.Get("Reserved Role") as string;

            Role role = null;
            if (type != null) role = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as Role;
            if (role) settings.Set("Objective Role", role);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey("Objective Role"))
            {
                var role = settings.Get("Objective Role") as Role;
                if (role && role.Reserve(gameObject)) done(this);
                else fail(this);
            }
            else fail(this);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            base.Exit(next);
        }
    }
}