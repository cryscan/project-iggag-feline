using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GameOverReturn : Action
{
    public int index;

    public override TaskStatus OnUpdate()
    {
        GameManager.instance.GameOverReturn(index);
        return TaskStatus.Success;
    }
}