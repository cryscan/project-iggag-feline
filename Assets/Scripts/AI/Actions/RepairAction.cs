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

        string type = "RepairRole";

        BehaviorTree behavior;

        protected override void Awake()
        {
            behavior = GetComponent<BehaviorTree>();
            base.Awake();

            preconditions.Set($"Reserved Role Type", type);
            effects.Set($"Has Role {type}", false);
        }

        public override ReGoapState<string, object> GetPreconditions(GoapActionStackData<string, object> stackData)
        {
            var role = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as RepairRole;
            if (role) preconditions.Set("At Position", role.transform.position);

            return base.GetPreconditions(stackData);
        }

        public override List<ReGoapState<string, object>> GetSettings(GoapActionStackData<string, object> stackData)
        {
            var role = agent.GetMemory().GetWorldState().Get($"Nearest {type}") as RepairRole;
            settings.Set($"Objective {type}", role);

            return base.GetSettings(stackData);
        }

        public override void Run(IReGoapAction<string, object> previous, IReGoapAction<string, object> next, ReGoapState<string, object> settings, ReGoapState<string, object> goalState, Action<IReGoapAction<string, object>> done, Action<IReGoapAction<string, object>> fail)
        {
            base.Run(previous, next, settings, goalState, done, fail);

            if (settings.HasKey($"Objective {type}"))
            {
                var role = settings.Get($"Objective {type}") as RepairRole;
                if (role)
                {
                    _light.enabled = false;

                    behavior.ExternalBehavior = external;
                    behavior.SetVariableValue("Target", role.breakable.gameObject);

                    StartCoroutine(ActionCheckCoroutine(role));
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

        IEnumerator RepairCoroutine(RepairRole role)
        {
            while (true)
            {
                role.breakable.Repair();
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator ActionCheckCoroutine(RepairRole role)
        {
            while (true)
            {
                if (!role.breakable.broken) doneCallback(this);
                else if (!role.enabled) doneCallback(this);
                else if (!role.IsReserved(gameObject)) failCallback(this);

                yield return null;
            }
        }
    }
}
