using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Actions
{
    public class ReserveRoleAction : ReGoapAction<string, object>
    {
        [SerializeField] string role;

        protected override void Awake()
        {
            base.Awake();

            preconditions.Set($"Has Role {role}", true);
            effects.Set($"Reserved {role}", true);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            var state = stackData.goalState;
            if (state.HasKey(role)) effects.Set(role, state.Get(role));

            return base.GetEffects(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var state = stackData.goalState;
            if (state.HasKey(role)) settings.Set($"Objective {role}", state.Get(role));

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            var key = $"Objective {role}";
            if (settings.HasKey(key))
            {
                var role = settings.Get(key) as Role;
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

        public override string ToString()
        {
            return $"GoapAction({Name}, {role})";
        }
    }
}