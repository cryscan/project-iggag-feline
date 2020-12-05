using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnterPlayScene : Action
{
    public string name;
    public bool fade;

    public override TaskStatus OnUpdate()
    {
        GameManager.instance.EnterSceneRelocate(name, GameState.Play, fade);
        return TaskStatus.Success;
    }
}