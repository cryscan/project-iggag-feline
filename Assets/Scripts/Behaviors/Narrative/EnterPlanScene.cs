using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnterPlanScene : Action
{
    public string name;
    public bool fade;

    public override TaskStatus OnUpdate()
    {
        GameManager.instance.EnterPlanSceneRelocate(name, fade);
        return TaskStatus.Success;
    }
}