using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject container;

    bool active = false;

    DialogueController dialogueController;

    void Awake()
    {
        dialogueController = GameObject.FindWithTag("Dialogue Controller").GetComponent<DialogueController>();
        container.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !dialogueController.running)
        {
            GameManager.instance.TogglePause();
            container.SetActive(!active);
            active = !active;
        }

        if (Input.GetKeyDown(KeyCode.R) && GameManager.instance.currentState != GameState.Paused)
        {
            Scene scene = SceneManager.GetActiveScene();
            GameManager.instance.EnterPlanScene(scene.name);
        }
    }

    public void MainMenuButton()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        GameManager.instance.TogglePause();
        GameManager.instance.GameOverReturn();
    }

    public void RestartStage()
    {
        GameManager.instance.TogglePause();
        Scene scene = SceneManager.GetActiveScene();
        GameManager.instance.EnterPlanScene(scene.name);
    }
}
