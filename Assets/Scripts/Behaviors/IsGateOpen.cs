using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsGateOpen : Conditional
{
    public GateReaction gate;

    public override TaskStatus OnUpdate()
    {
        return gate.open ? TaskStatus.Success : TaskStatus.Failure;
    }
}