using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorDesigner.Runtime;

using ReGoap.Core;
using ReGoap.Unity;
using System;

namespace Feline.AI.Actions
{
    public class RepairAction : ReGoapAction<string, object>
    {
        [SerializeField] ExternalBehavior external;
        [SerializeField] Light _light;

        string type = "RepairPoint";

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();
            base.Awake();

            preconditions.Set($"Reserved Role", type);
            effects.Set($"Has Role {type}", false);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var repairPoint = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as RepairPoint;
            if (repairPoint) preconditions.Set("At Position", repairPoint.transform.position);

            return base.GetPreconditions(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var repairPoint = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as RepairPoint;
            settings.Set($"Objective {type}", repairPoint);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey($"Objective {type}"))
            {
                var repairPoint = settings.Get($"Objective {type}") as RepairPoint;
                if (repairPoint)
                {
                    _light.enabled = false;

                    behavior.ExternalBehavior = external;
                    StartCoroutine(RepairCoroutine(repairPoint));
                    StartCoroutine(ActionCheckCoroutine(repairPoint));
                }
                else fail(this);
            }
            else fail(this);
        }

        public override void Exit(IReGoapAction<string, object> next)
        {
            StopAllCoroutines();
            _light.enabled = true;

            base.Exit(next);
        }

        IEnumerator RepairCoroutine(RepairPoint repairPoint)
        {
            while (true)
            {
                repairPoint.breakable.Repair();
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator ActionCheckCoroutine(RepairPoint repairPoint)
        {
            while (true)
            {
                if (!repairPoint.breakable.broken) doneCallback(this);
                else if (!repairPoint.valid) doneCallback(this);
                else if (!repairPoint.IsReserved(gameObject)) failCallback(this);

                yield return null;
            }
        }
    }
}
