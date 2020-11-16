using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnterPlanScene : Action
{
    public string name;
    public int index;

    public override TaskStatus OnUpdate()
    {
        MainMenuController.UnlockLevel(index);
        GameManager.instance.EnterPlanSceneRelocate(name);
        return TaskStatus.Success;
    }
}