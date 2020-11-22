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
    public class ChaseAction : ReGoapAction<string, object>
    {

        [UnityEngine.Tooltip("Should be Chase behavior")]
        [SerializeField] ExternalBehaviorTree external;

        [SerializeField] GameObject _audio;
        [SerializeField] float speed = 6;

        BehaviorTree behavior;

        GameObject player;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();
            player = GameObject.FindWithTag("Player");

            base.Awake();

            preconditions.Set("Can See Player", true);

            effects.Set("Player Dead", true);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            behavior.ExternalBehavior = external;
            behavior.SetVariableValue("Target", player);
            behavior.SetVariableValue("Audio", _audio);
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
                bool canSeePlayer = (bool)state.Get("Can See Player");
                if (!canSeePlayer) failCallback(this);
                yield return null;
            }
        }
    }
}