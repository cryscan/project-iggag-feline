using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Unity;

namespace Feline.AI.Goals
{
    public class SimpleGoal : ReGoapGoal<string, object>
    {
        protected override void Awake()
        {
            base.Awake();

            var player = GameObject.FindGameObjectWithTag("Player");
            goal.Set("At", player.transform.position);
        }
    }
}
