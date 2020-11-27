using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public bool pause = false;
    public string[] sentences;
}

public class DialogueController : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] Text text;

    public bool running { get; private set; } = false;

    Queue<string> sentences;
    string currentSentence;

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
        if (container.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!typing) DisplayNextSentence();
            else DisplayCurrentSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        running = true;

        // Pause Game here
        // GameManager.instance.TogglePause();

        container.SetActive(true);

        sentences.Clear();
        foreach (string sentence in dialogue.sentences) sentences.Enqueue(sentence);

        // EventBus.Publish(new DialogueEvent(true, dialogue.topic));

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
        running = false;
        currentSentence = null;

        // EventBus.Publish(new DialogueEvent(false, topic));

        // Resume Game here
        // GameManager.instance.TogglePause();
        // canvas.GetComponent<GraphicRaycaster>().enabled = false;

        Debug.Log("[Dialogue] end of conversation");

        container.SetActive(false);
    }
}
