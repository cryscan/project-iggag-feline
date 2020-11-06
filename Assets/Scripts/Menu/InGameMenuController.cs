using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
	public GameObject canvas;
	public GameObject dialogueParent;
	public bool active = false;
    public bool dialogueOccuring = false;
    Subscription<DialogueEvent> dialogueEventHandler;

    void Awake()
    {
        dialogueEventHandler = EventBus.Subscribe<DialogueEvent>(OnDialogueEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !dialogueOccuring)
        {
            if(!active)
            {
            	GameManager.instance.TogglePause();
        		canvas.GetComponent<GraphicRaycaster>().enabled = true;
            	dialogueParent.SetActive(true);
            	active = true;
            }
            else if(active)
            {
                GameManager.instance.TogglePause();
                canvas.GetComponent<GraphicRaycaster>().enabled = false;
                dialogueParent.SetActive(false);
                active = false;
            }
        }   
    }

    void OnDialogueEvent(DialogueEvent @event)
    {
        dialogueOccuring = @event.dialogueOccuring;
    }

    public void MainMenuButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void RestartStage() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
