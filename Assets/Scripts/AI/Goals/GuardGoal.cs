﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ReGoap.Core;
using ReGoap.Unity;

namespace Feline.AI.Goals
{
    public class GuardGoal : ReGoapGoalAdvanced<string, object>
    {
        protected override void Awake()
        {
            base.Awake();
            goal.Set("Player Dead", true);
        }
    }
}
