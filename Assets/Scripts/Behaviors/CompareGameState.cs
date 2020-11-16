using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CompareGameState : Conditional
{
    public GameState state;

    public override TaskStatus OnUpdate()
    {
        return state == GameManager.instance.currentState ? TaskStatus.Success : TaskStatus.Failure;
    }
}