using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] GameObject container;

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
            if (active) GameManager.instance.PushPauseState();
            else GameManager.instance.PopPauseState();
        }

        if (Input.GetKeyDown(KeyCode.R) && GameManager.instance.currentState != GameState.Paused)
        {
            Scene scene = SceneManager.GetActiveScene();
            GameManager.instance.EnterPlanScene(scene.name);
        }
    }

    public void MainMenuButton(int index = 0)
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        GameManager.instance.PopPauseState();
        GameManager.instance.GameOverReturn(index);
    }

    public void RestartStage()
    {
        GameManager.instance.PopPauseState();
        Scene scene = SceneManager.GetActiveScene();
        GameManager.instance.EnterPlanScene(scene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
