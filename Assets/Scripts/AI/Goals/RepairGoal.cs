using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Goals
{
    public class RepairGoal : ReGoapGoalAdvanced<string, object>
    {
        IReGoapAgent<string, object> agent;

        protected override void Awake()
        {
            agent = GetComponent<IReGoapAgent<string, object>>();

            base.Awake();
            goal.Set("Has Role RepairPoint", false);
        }

        public override bool IsGoalPossible()
        {
            var state = agent.GetMemory().GetWorldState();
            if (state.HasKey("Has Role RepairPoint"))
            {
                bool hasRepairPoint = (bool)state.Get("Has Role RepairPoint");
                return hasRepairPoint;
            }
            return base.IsGoalPossible();
        }
    }
}
