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

    void Awake()
    {
        if (sameScene) playSceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        ScheduleManager.instance.SetMaxTime(_maxPlanTime);
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
                game.EnterPlaySceneRelocate(playSceneName, true);
        }
    }
}
