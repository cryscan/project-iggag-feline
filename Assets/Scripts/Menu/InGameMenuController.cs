using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject container;

    bool active = false;
    bool dialogueOccuring = false;
    Subscription<DialogueEvent> handler;

    void Awake()
    {
        container.SetActive(false);
    }

    void OnEnable()
    {
        handler = EventBus.Subscribe<DialogueEvent>(OnDialogue);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !dialogueOccuring)
        {
            GameManager.instance.TogglePause();
            container.SetActive(!active);
            active = !active;
        }
    }

    void OnDialogue(DialogueEvent @event)
    {
        dialogueOccuring = @event.starting;
    }

    public void MainMenuButton()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        GameManager.instance.GameOverReturn();
    }

    public void RestartStage()
    {
        Scene scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.name);
        GameManager.instance.RestartPlanScene(scene.name);
    }
}
