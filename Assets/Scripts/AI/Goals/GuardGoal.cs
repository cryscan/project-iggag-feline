using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Goals
{
    public class GuardGoal : ReGoapGoalAdvanced<string, object>
    {
        IReGoapAgent<string, object> agent;

        protected override void Awake()
        {
            base.Awake();
            goal.Set("Player Dead", true);

            agent = GetComponent<IReGoapAgent<string, object>>();
        }

        public override bool IsGoalPossible()
        {
            var state = agent.GetMemory().GetWorldState();
            if (state.Get("Frozen") != null)
            {
                bool frozen = (bool)state.Get("Frozen");
                return !frozen;
            }
            return true;
        }
    }
}
