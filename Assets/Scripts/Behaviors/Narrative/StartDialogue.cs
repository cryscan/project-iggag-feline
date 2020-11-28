using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class StartDialogue : Action
{
    public TextAsset text;
    public bool pause;

    Dialogue dialogue = new Dialogue();

    public override void OnAwake()
    {
        string[] input = text.text.Split('\n');
        dialogue.sentences = input;
        dialogue.pause = pause;
    }

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