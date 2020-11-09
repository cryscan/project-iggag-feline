using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TextAsset file;

    DialogueController controller;
    Subscription<GameStateChangeEvent> handler;

    // Start is called before the first frame update
    void Awake()
    {
        string[] input = file.text.Split('\n');
        dialogue = new Dialogue();
        dialogue.sentences = input;

        controller = GetComponent<DialogueController>();
    }

    void OnEnable()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.previous != GameState.Paused && @event.current == GameState.Plan)
        {

            controller.StartDialogue(dialogue);
        }
    }
}
