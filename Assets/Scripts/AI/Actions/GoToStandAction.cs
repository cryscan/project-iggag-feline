using System.Collections;
using System.Collections.Generic;
using ReGoap.Core;
using UnityEngine;

namespace Feline.AI.Actions
{
    public class GoToStandAction : GoToAction
    {
        string role;

        protected override void Awake()
        {
            role = typeof(StandPoint).ToString();

            base.Awake();

            preconditions.Set($"Has Available {role}", true);
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
            if (settings.HasKey($"Objective {role}"))
            {
                var standPoint = settings.Get($"Objective {role}") as StandPoint;
                if (standPoint)
                {
                    if (!standPoint.Reserve(gameObject)) fail(this);
                    StartCoroutine(ActionCheckCoroutine(standPoint));
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

        IEnumerator ActionCheckCoroutine(StandPoint standPoint)
        {
            while (true)
            {
                if (!standPoint.valid || standPoint.reservation != gameObject) failCallback(this);
                yield return null;
            }
        }
    }
}