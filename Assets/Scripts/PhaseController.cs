using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseController : MonoBehaviour
{
    [SerializeField] int _maxPlanTime = 60;
    public int maxPlanTime { get => _maxPlanTime; }

    [SerializeField] bool hasTimeLimit = true;

    bool fast = false;

    [SerializeField] AudioSource fastStartAudio, fastEndAudio;
    [SerializeField] GameObject fastAudio;

    void Start()
    {
        ScheduleManager.instance.SetMaxTime(_maxPlanTime);
        GameManager.instance.SetFast(false);
    }

    void Update()
    {
        var game = GameManager.instance;

        /*
        if (game.currentState == GameState.Plan && ScheduleManager.instance.timer > _maxPlanTime)
            game.TogglePause();
        */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (game.currentState == GameState.Plan && !game.transiting)
                game.RestartCurrentScene(GameState.Play);
        }

        if (hasTimeLimit && ScheduleManager.instance.timer > maxPlanTime)
            game.RestartCurrentScene(GameState.Play);

        if (GameManager.instance.currentState == GameState.Plan && Input.GetKeyDown(KeyCode.E))
        {
            fast = !fast;
            GameManager.instance.SetFast(fast);

            if (fast) fastStartAudio.Play();
            else fastEndAudio.Play();
        }

        fastAudio.SetActive(fast);
    }
}
