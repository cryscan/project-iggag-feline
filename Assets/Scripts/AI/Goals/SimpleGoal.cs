using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Unity;

namespace Feline.AI.Goals
{
    public class SimpleGoal : ReGoapGoal<string, object>
    {
        [SerializeField] Vector3 destination;

        protected override void Awake()
        {
            base.Awake();

            goal.Set("At", destination);
        }
    }
}
