using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    public bool running
    {
        get
        {
            if (!controller) return false;
            else return controller.running;
        }
    }

    DialogueController controller;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterController(DialogueController controller) => this.controller = controller;

    public void StartDialogue(Dialogue dialogue) => controller.StartDialogue(dialogue);
}
