using System.Collections;
using System.Collections.Generic;
using ReGoap.Core;
using UnityEngine;

namespace Feline.AI.Actions
{
    public class ReserveGoToAction : GoToAction
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
            var key = $"Objective {role}";
            if (settings.HasKey(key))
            {
                var role = settings.Get(key) as Role;
                if (role)
                {
                    if (!role.Reserve(gameObject)) fail(this);
                    StartCoroutine(ActionCheckCoroutine(role));
                }
                else fail(this);
            }
            else fail(this);

            base.Run(previous, next, settings, goalState, done, fail);
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

        IEnumerator ActionCheckCoroutine(Role role)
        {
            while (true)
            {
                if (!role.valid || !role.IsReserved(gameObject)) failCallback(this);
                yield return null;
            }
        }
    }
}