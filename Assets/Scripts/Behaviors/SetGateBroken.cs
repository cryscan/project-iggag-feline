using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SetGateBroken : Action
{
    public GateReaction gate;

    public override TaskStatus OnUpdate()
    {
        gate.SetBroken();
        return TaskStatus.Success;
    }
}