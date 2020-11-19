using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BreakGate : Action
{
    public GateController gate;

    public override TaskStatus OnUpdate()
    {
        gate.Break();
        return TaskStatus.Success;
    }
}