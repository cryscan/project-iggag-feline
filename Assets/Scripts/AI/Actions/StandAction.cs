using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;
using ReGoap.Core;

namespace Feline.AI.Actions
{
    public class StandAction : ReGoapAction<string, object>
    {
        [Tooltip("Should be Stand action")]
        [SerializeField] ExternalBehaviorTree external;

        BehaviorTree behavior;

        string role = "StandPoint";

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();
            preconditions.Set("Alerted", false);
            preconditions.Set("Can See Player", false);

            preconditions.Set($"Reserved {role}", true);
            effects.Set("Can See Player", true);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var standPoint = agent.GetMemory().GetWorldState().Get($"Nearest {role}") as StandPoint;
            if (standPoint)
            {
                preconditions.Set(role, standPoint);
                preconditions.Set("At Position", standPoint.transform.position);
            }
            return base.GetPreconditions(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var standPoint = agent.GetMemory().GetWorldState().Get($"Nearest {role}") as StandPoint;
            settings.Set($"Objective {role}", standPoint);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey($"Objective {role}"))
            {
                var standPoint = settings.Get($"Objective {role}") as StandPoint;
                if (standPoint)
                {
                    behavior.ExternalBehavior = external;
                    behavior.SetVariableValue("Points", standPoint.points);

                    StartCoroutine(ActionCheckCoroutine(standPoint));
                }
                else fail(this);
            }
            else fail(this);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            base.Exit(next);
        }

        IEnumerator ActionCheckCoroutine(StandPoint standPoint)
        {
            var state = agent.GetMemory().GetWorldState();
            while (true)
            {
                if (!standPoint.valid || !standPoint.IsReserved(gameObject)) failCallback(this);

                bool canSeePlayer = (bool)state.Get("Can See Player");
                bool alerted = (bool)state.Get("Alerted");

                if (canSeePlayer) doneCallback(this);
                else if (alerted) failCallback(this);

                yield return null;
            }
        }
    }
}