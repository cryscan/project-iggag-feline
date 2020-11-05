using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public Text dialogueText;

	public Queue<string> sentences;

	public GameObject canvas;

	public GameObject dialogueParent;

    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
    	//Pause Game here
    	GameManager.instance.TogglePause();
    	canvas.GetComponent<GraphicRaycaster>().enabled = true;

    	sentences.Clear();
    	foreach (string sentence in dialogue.sentences) {
    		sentences.Enqueue(sentence);
    	}
    	DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
    	if (sentences.Count == 0) {
    		EndDialogue();
    		return;
    	}
    	string sentence = sentences.Dequeue();
    	StopAllCoroutines();
    	StartCoroutine(TypeSentence(sentence));
    	Debug.Log("SENTENCE DISPLAYED");
    }

    IEnumerator TypeSentence(string sentence)
    {
    	dialogueText.text = "";
    	foreach (char letter in sentence.ToCharArray()) {
    		dialogueText.text += letter;
    		yield return WaitForRealSeconds(0.025f);
    	}
    }

    IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    public void EndDialogue()
    {
    	Debug.Log("End of conversation");
    	//Unpause Game here
    	GameManager.instance.TogglePause();
    	canvas.GetComponent<GraphicRaycaster>().enabled = false;
    	dialogueParent.SetActive(false);
    }
}
