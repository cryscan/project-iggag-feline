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

            pathPointsParent.parent = null;
            var query = from i in Enumerable.Range(0, pathPointsParent.childCount)
                        select pathPointsParent.GetChild(i).gameObject;
            pathPoints = query.ToList();

            base.Awake();

            preconditions.Set("Alerted", false);
            preconditions.Set("Can See Player", false);

            effects.Set("Can See Player", true);
        }

        void OnEnable()
        {
            behavior.OnBehaviorEnd += OnBehaviorEnded;
        }

        void OnDisable()
        {
            behavior.OnBehaviorEnd -= OnBehaviorEnded;
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;
            behavior.SetVariableValue("Path Points", pathPoints);
            behavior.SetVariableValue("Speed", speed);
        }

        void OnBehaviorEnded(Behavior behavior)
        {
            if (behavior.ExecutionStatus == TaskStatus.Failure)
            {
                // Debug.Break();
                doneCallback(this);
            }
        }
    }
}
