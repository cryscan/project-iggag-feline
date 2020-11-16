using UnityEngine;
using UnityEngine.SceneManagement;

using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class UnlockScene : Action
{
    public override TaskStatus OnUpdate()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        MainMenuController.UnlockLevel(index - 1);

        return TaskStatus.Success;
    }
}