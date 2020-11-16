using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnterPlayScene : Action
{
    public string name;
    public int index;

    public override TaskStatus OnUpdate()
    {
        MainMenuController.UnlockLevel(index);
        GameManager.instance.EnterPlaySceneRelocate(name);
        return TaskStatus.Success;
    }
}