using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsGateOpen : Conditional
{
    public GateController gate;

    public override TaskStatus OnUpdate()
    {
        return gate.open ? TaskStatus.Success : TaskStatus.Failure;
    }
}