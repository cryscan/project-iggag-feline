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

    AudioSource audioSource;

    void Awake()
    {
        sentences = new Queue<string>();
        container.SetActive(false);

        audioSource = GetComponent<AudioSource>();
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
        if (dialogue.pause) GameManager.instance.PushState(GameState.Paused);

        if (dialogue.audio)
        {
            audioSource.clip = dialogue.audio;
            audioSource.volume = dialogue.volume;
            audioSource.pitch = dialogue.pitch;
            audioSource.Play();
        }

        if (dialogue.voice)
        {
            StopAllCoroutines();
            StartCoroutine(PlayVoiceCoroutine());
        }
        else
        {
            container.SetActive(true);

            sentences.Clear();
            foreach (string sentence in dialogue.sentences) sentences.Enqueue(sentence);

            DisplayNextSentence();
        }
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
        StartCoroutine(TypeSentenceCoroutine(currentSentence));

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

    IEnumerator TypeSentenceCoroutine(string sentence)
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

    IEnumerator PlayVoiceCoroutine()
    {
        var duration = dialogue.audio.length;
        yield return new WaitForSeconds(duration);

        EndDialogue();
    }

    public void EndDialogue()
    {
        if (!dialogue) return;

        if (dialogue.pause)
            GameManager.instance.PopPauseState();

        dialogue = null;
        currentSentence = null;
        running = false;

        Debug.Log("[Dialogue] end of conversation");

        container.SetActive(false);
    }
}