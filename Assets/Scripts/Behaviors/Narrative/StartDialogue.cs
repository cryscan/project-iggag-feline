using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class StartDialogue : Action
{
    public Dialogue dialogue;

    public override void OnStart()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    public override TaskStatus OnUpdate()
    {
        if (DialogueManager.instance.running) return TaskStatus.Running;
        else return TaskStatus.Success;
    }
}