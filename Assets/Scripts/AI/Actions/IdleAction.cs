using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Unity;
using ReGoap.Core;

namespace Feline.AI.Actions
{
    [RequireComponent(typeof(BehaviorTree))]
    public class IdleAction : ReGoapAction<string, object>
    {
        [UnityEngine.Tooltip("Should be Idle behavior")]
        [SerializeField] ExternalBehaviorTree external;

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;
            behavior.EnableBehavior();
            doneCallback(this);
        }
    }
}
