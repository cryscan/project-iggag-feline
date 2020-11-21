using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Actions
{
    public class SearchAction : ReGoapAction<string, object>
    {
        [UnityEngine.Tooltip("Should be Search behavior")]
        [SerializeField] ExternalBehaviorTree external;
        [SerializeField] float speed = 4;
        [SerializeField] float subspeed = 2;

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();

            base.Awake();

            preconditions.Set("Alerted", true);
            preconditions.Set("Can See Player", false);

            effects.Set("Can See Player", true);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            var state = agent.GetMemory().GetWorldState();
            if (state.HasKey("Spotted Position"))
            {
                behavior.ExternalBehavior = external;
                behavior.SetVariableValue("Speed", speed);
                behavior.SetVariableValue("Subspeed", subspeed);

                StopAllCoroutines();
                StartCoroutine(ActionCheckCoroutine());
            }
            else failCallback(this);
        }

        IEnumerator ActionCheckCoroutine()
        {
            var state = agent.GetMemory().GetWorldState();

            while (true)
            {
                Vector3 position = (Vector3)state.Get("Spotted Position");
                behavior.SetVariableValue("Position", position);

                bool alerted = (bool)state.Get("Alerted");
                bool canSeePlayer = (bool)state.Get("Can See Player");

                if (!alerted) failCallback(this);
                if (canSeePlayer) doneCallback(this);

                yield return null;
            }
        }
    }
}
