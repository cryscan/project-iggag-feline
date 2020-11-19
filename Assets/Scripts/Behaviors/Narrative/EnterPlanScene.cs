using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnterPlanScene : Action
{
    public string name;

    public override TaskStatus OnUpdate()
    {
        GameManager.instance.EnterPlanSceneRelocate(name);
        return TaskStatus.Success;
    }
}