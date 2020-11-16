using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CompareGameState : Conditional
{
    public GameState state;

    public override TaskStatus OnUpdate()
    {
        if (state == GameManager.instance.currentState) return TaskStatus.Success;
        else return TaskStatus.Failure;
    }
}