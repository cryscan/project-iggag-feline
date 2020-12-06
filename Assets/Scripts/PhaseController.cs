using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseController : MonoBehaviour
{
    [SerializeField] string playSceneName;
    [SerializeField] bool sameScene = true;

    [SerializeField] int _maxPlanTime = 60;
    public int maxPlanTime { get => _maxPlanTime; }

    [SerializeField] bool hasTimeLimit = true;

    bool fast = false;

    void Awake()
    {
        if (sameScene) playSceneName = SceneManager.GetActiveScene().name;
    }

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
                game.EnterSceneRelocate(playSceneName, GameState.Play, true);
        }

        if (hasTimeLimit && ScheduleManager.instance.timer > maxPlanTime)
            game.EnterSceneRelocate(playSceneName, GameState.Play, true);

        if (GameManager.instance.currentState == GameState.Plan && Input.GetKeyDown(KeyCode.E))
        {
            fast = !fast;
            GameManager.instance.SetFast(fast);
        }
    }
}
