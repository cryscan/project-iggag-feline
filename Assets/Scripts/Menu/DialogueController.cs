using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string topic;
    public string[] sentences;
}

public class DialogueEvent
{
    public bool starting;
    public string topic;

    public DialogueEvent(bool starting, string topic)
    {
        this.starting = starting;
        this.topic = topic;
    }
}

public class DialogueController : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] Text text;

    public bool running { get; private set; } = false;

    string topic;
    Queue<string> sentences;

    void Awake()
    {
        sentences = new Queue<string>();
        container.SetActive(false);
    }

    void LateUpdate()
    {
        if (container.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        running = true;

        // Pause Game here
        // GameManager.instance.TogglePause();

        container.SetActive(true);
        topic = dialogue.topic;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences) sentences.Enqueue(sentence);

        EventBus.Publish(new DialogueEvent(true, dialogue.topic));

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log("[Dialogue] sentence displayed");
    }

    IEnumerator TypeSentence(string sentence)
    {
        text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSecondsRealtime(0.005f);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("[Dialogue] end of conversation");

        running = false;
        EventBus.Publish(new DialogueEvent(false, topic));

        topic = "";

        // Resume Game here
        // GameManager.instance.TogglePause();
        // canvas.GetComponent<GraphicRaycaster>().enabled = false;

        container.SetActive(false);
    }
}
