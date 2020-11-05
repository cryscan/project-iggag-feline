using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;
	public TextAsset file;
	Subscription<GameStateChangeEvent> gameStateChangeHandler;
	public GameObject dialogueParent;
	
    // Start is called before the first frame update
    void Awake()
    {
    	string[] input = file.text.Split('\n');
    	dialogue = new Dialogue();
    	dialogue.sentences = input;
    	gameStateChangeHandler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    	dialogueParent.SetActive(false); 
    }

    public void TriggerDialogue ()
    {
    	FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.previous == GameState.Start && @event.current == GameState.Plan)
        {
        	dialogueParent.SetActive(true);
        	TriggerDialogue();
        }
    }
}
