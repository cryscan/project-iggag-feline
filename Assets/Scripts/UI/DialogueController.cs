using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] Text text;

    public bool running { get; private set; } = false;

    Queue<string> sentences;
    string currentSentence;

    Dialogue dialogue;

    bool typing = false;

    void Awake()
    {
        sentences = new Queue<string>();
        container.SetActive(false);
    }

    void Start()
    {
        DialogueManager.instance.RegisterController(this);
    }

    void LateUpdate()
    {
        if (running)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!typing) DisplayNextSentence();
                else DisplayCurrentSentence();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue.triggered && dialogue.once) return;

        if (running) EndDialogue();

        running = true;
        this.dialogue = dialogue;

        dialogue.triggered = true;

        // Pause Game here
        // GameManager.instance.TogglePause();

        container.SetActive(true);

        sentences.Clear();
        foreach (string sentence in dialogue.sentences) sentences.Enqueue(sentence);

        // EventBus.Publish(new DialogueEvent(true, dialogue.topic));
        if (dialogue.pause) GameManager.instance.PushState(GameState.Paused);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));

        Debug.Log("[Dialogue] sentence displayed");
    }

    public void DisplayCurrentSentence()
    {
        if (currentSentence == null)
        {
            Debug.LogError("[Dialogue] no current sentence");
            return;
        }

        StopAllCoroutines();
        typing = false;
        text.text = currentSentence;
    }

    IEnumerator TypeSentence(string sentence)
    {
        typing = true;
        text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSecondsRealtime(0.005f);
        }
        typing = false;
    }

    public void EndDialogue()
    {
        if (dialogue.pause) GameManager.instance.PopPauseState();
        dialogue = null;

        currentSentence = null;

        running = false;

        // EventBus.Publish(new DialogueEvent(false, topic));

        // Resume Game here
        // GameManager.instance.TogglePause();
        // canvas.GetComponent<GraphicRaycaster>().enabled = false;

        Debug.Log("[Dialogue] end of conversation");

        container.SetActive(false);
    }
}
