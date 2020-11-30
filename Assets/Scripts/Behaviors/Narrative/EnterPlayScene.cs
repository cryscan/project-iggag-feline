using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnterPlayScene : Action
{
    public string name;
    public bool fade;

    public override TaskStatus OnUpdate()
    {
        GameManager.instance.EnterPlaySceneRelocate(name, fade);
        return TaskStatus.Success;
    }
}