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

    string topic;
    Queue<string> sentences;

    void Awake()
    {
        sentences = new Queue<string>();
        container.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        EventBus.Publish(new DialogueEvent(true, dialogue.topic));

        // Pause Game here
        GameManager.instance.TogglePause();
        container.SetActive(true);
        // canvas.GetComponent<GraphicRaycaster>().enabled = true;

        topic = dialogue.topic;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences) sentences.Enqueue(sentence);
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
            yield return new WaitForSecondsRealtime(0.025f);
        }
    }

    public void EndDialogue()
    {
        EventBus.Publish(new DialogueEvent(false, topic));
        Debug.Log("[Dialogue] end of conversation");

        topic = "";

        // Resume Game here
        GameManager.instance.TogglePause();
        // canvas.GetComponent<GraphicRaycaster>().enabled = false;
        container.SetActive(false);
    }
}
