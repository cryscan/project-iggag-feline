using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Actions
{
    public class PatrolAction : ReGoapAction<string, object>
    {

        [UnityEngine.Tooltip("Should be Patrol behavior")]
        [SerializeField] ExternalBehaviorTree external;
        [SerializeField] Transform pathPointsParent;
        [SerializeField] float speed = 2;

        List<GameObject> pathPoints;

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            // pathPointsParent.parent = null;
            var query = from i in Enumerable.Range(0, pathPointsParent.childCount)
                        select pathPointsParent.GetChild(i).gameObject;
            pathPoints = query.ToList();

            base.Awake();

            preconditions.Set("Alerted", false);
            preconditions.Set("Can See Player", false);

            effects.Set("Can See Player", true);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;
            behavior.SetVariableValue("Path Point List", pathPoints);
            behavior.SetVariableValue("Speed", speed);

            StartCoroutine(ActionCheckCoroutine());
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            base.Exit(next);
        }

        IEnumerator ActionCheckCoroutine()
        {
            var state = agent.GetMemory().GetWorldState();
            while (true)
            {
                bool alerted = (bool)state.Get("Alerted");
                bool canSeePlayer = (bool)state.Get("Can See Player");

                if (canSeePlayer) doneCallback(this);
                else if (alerted) failCallback(this);

                yield return null;
            }
        }
    }
}
