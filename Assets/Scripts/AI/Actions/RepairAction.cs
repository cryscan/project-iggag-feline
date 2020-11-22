using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;
using System;

namespace Feline.AI.Actions
{
    public class RepairAction : ReGoapAction<string, object>
    {
        string role = "RepairPoint";

        protected override void Awake()
        {
            base.Awake();

            preconditions.Set($"Reserved {role}", true);
            effects.Set($"Has Role {role}", false);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var repairPoint = agent.GetMemory().GetWorldState().Get($"Nearest {role}") as RepairPoint;
            if (repairPoint)
            {
                preconditions.Set(role, repairPoint);
                preconditions.Set("At Position", repairPoint.transform.position);
            }
            return base.GetPreconditions(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var repairPoint = agent.GetMemory().GetWorldState().Get($"Nearest {role}") as RepairPoint;
            settings.Set($"Objective {role}", repairPoint);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey($"Objective {role}"))
            {
                var repairPoint = settings.Get($"Objective {role}") as RepairPoint;
                if (repairPoint) StartCoroutine(ActionCheckCoroutine(repairPoint));
                else fail(this);
            }
            else fail(this);
        }

        IEnumerator ActionCheckCoroutine(RepairPoint repairPoint)
        {
            while (true)
            {
                repairPoint.breakable.Repair();

                if (!repairPoint.breakable.broken) doneCallback(this);
                else if (!repairPoint.valid) doneCallback(this);
                else if (!repairPoint.IsReserved(gameObject)) failCallback(this);

                yield return null;
            }
        }
    }
}
