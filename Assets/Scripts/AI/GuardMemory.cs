using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReGoap.Unity;

namespace Feline.AI
{
    public class GuardMemory : ReGoapMemoryAdvanced<string, object>
    {
        protected override void Awake()
        {
            base.Awake();

            state.Set("Player Dead", false);
        }
    }
}
