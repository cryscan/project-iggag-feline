﻿using System;
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
    public class ChaseAction : ReGoapAction<string, object>
    {

        [UnityEngine.Tooltip("Should be Chase behavior")]
        [SerializeField] ExternalBehaviorTree external;

        [SerializeField] float speed = 6;

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();

            preconditions.Set("Can See Player", true);
            effects.Set("Player Dead", true);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;
            behavior.SetVariableValue("Speed", speed);

            StopAllCoroutines();
            StartCoroutine(ActionCheckCoroutine());
        }

        IEnumerator ActionCheckCoroutine()
        {
            var state = agent.GetMemory().GetWorldState();

            while (true)
            {
                bool canSeePlayer = (bool)state.Get("Can See Player");
                bool playerDead = (bool)state.Get("Player Dead");

                if (!canSeePlayer) failCallback(this);
                if (playerDead) doneCallback(this);

                yield return null;
            }
        }
    }
}