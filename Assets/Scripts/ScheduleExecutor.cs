using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScheduleExecutor : MonoBehaviour
{
    Subscription<ScheduleDeployEvent> handler;

    void OnEnable()
    {
        handler = EventBus.Subscribe<ScheduleDeployEvent>(OnScheduleDeployed);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void Start()
    {
        ScheduleManager.instance.ResetTimer();

        var state = GameManager.instance.currentState;
        var sceneName = SceneManager.GetActiveScene().name;
        if (state == GameState.Plan)
        {
            ScheduleManager.instance.SetSceneName(sceneName);
            ScheduleManager.instance.ClearSchedule();
        }
        else if (state == GameState.Play && ScheduleManager.instance.sceneName == sceneName)
            ScheduleManager.instance.DeploySchedule();
    }

    public void Execute()
    {
        foreach (var schedule in ScheduleManager.instance.schedules)
            StartCoroutine(ScheduleExecuteCoroutine(schedule));
    }

    void OnScheduleDeployed(ScheduleDeployEvent @event) => Execute();

    IEnumerator ScheduleExecuteCoroutine(ScheduleTimerEvent @event)
    {
        var timer = @event.timer - ScheduleManager.instance.timer;
        if (timer < 0) yield break;
        yield return new WaitForSeconds(timer);

        EventBus.Publish(@event);
    }
}
