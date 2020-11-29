using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Goals
{
    public class CarryGoal : ReGoapGoalAdvanced<string, object>
    {
        IReGoapAgent<string, object> agent;

        string type = "CarryRole";

        protected override void Awake()
        {
            agent = GetComponent<IReGoapAgent<string, object>>();

            base.Awake();
            goal.Set($"Has Role {type}", false);
        }

        public override bool IsGoalPossible()
        {
            var state = agent.GetMemory().GetWorldState();
            if (state.HasKey($"Has Role {type}")) return (bool)state.Get($"Has Role {type}");
            return false;
        }
    }
}
