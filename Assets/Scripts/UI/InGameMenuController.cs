using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] bool restartable = true;

    bool active = false;

    void Awake()
    {
        container.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !DialogueManager.instance.running)
        {
            active = !active;
            container.SetActive(active);
            if (active) GameManager.instance.PushState(GameState.Paused);
            else GameManager.instance.PopPauseState();
        }

        var state = GameManager.instance.currentState;
        if (Input.GetKeyDown(KeyCode.R) && restartable && state != GameState.Paused)
            GameManager.instance.RestartCurrentScene(state);
    }

    public void MainMenuButton(int index = 0)
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        GameManager.instance.PopPauseState();
        GameManager.instance.GameOverReturn(index, true);
    }

    public void RestartStage()
    {
        GameManager.instance.PopPauseState();
        GameManager.instance.RestartCurrentScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
