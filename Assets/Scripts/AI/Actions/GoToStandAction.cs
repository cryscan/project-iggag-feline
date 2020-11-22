using System.Collections;
using System.Collections.Generic;
using ReGoap.Core;
using UnityEngine;

namespace Feline.AI.Actions
{
    public class GoToStandAction : GoToAction
    {
        protected override void Awake()
        {
            base.Awake();

            preconditions.Set("Has Available Stand Point", true);
            effects.Set("Reserved Stand Point", true);
        }

        public override ReGoapState<string, object> GetEffects(GoapActionStackData<string, object> stackData)
        {
            var key = "Stand Point";
            var state = stackData.goalState;
            if (state.HasKey(key)) effects.Set(key, state.Get(key));

            return base.GetEffects(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var state = stackData.goalState;
            if (state.HasKey("Stand Point")) settings.Set("Objective Stand Point", state.Get("Stand Point"));

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, System.Action<IReGoapAction<string, object>> done, System.Action<IReGoapAction<string, object>> fail)
        {
            if (settings.HasKey("Objective Stand Point"))
            {
                var standPoint = settings.Get("Objective Stand Point") as StandPoint;
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