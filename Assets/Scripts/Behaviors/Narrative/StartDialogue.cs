using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class StartDialogue : Action
{
    public TextAsset text;

    DialogueController controller;
    Dialogue dialogue = new Dialogue();

    public override void OnAwake()
    {
        controller = GameObject.FindWithTag("Dialogue Controller").GetComponent<DialogueController>();

        string[] input = text.text.Split('\n');
        dialogue.sentences = input;
    }

    public override void OnStart()
    {
        controller.StartDialogue(dialogue);
    }

    public override TaskStatus OnUpdate()
    {
        if (controller.running) return TaskStatus.Running;
        else return TaskStatus.Success;
    }
}