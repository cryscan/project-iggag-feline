using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseController : MonoBehaviour
{
    [SerializeField] float planningPhaseDuration = 15.0f;
    float timer;
    bool started;

    void Start()
    {
        EventBus.Publish<PlanningPhaseEvent>(new PlanningPhaseEvent(planningPhaseDuration));
        Debug.Log("Timer started! PlanningPhaseEvent published");
        timer = planningPhaseDuration;
        started = false;
    }

    void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            if (!started)
            {
                started = true;
                EventBus.Publish<GamePhaseEvent>(new GamePhaseEvent());
                Debug.Log("Timer ended! GamePhaseEvent published");
            }
        }
    }
}

public class PlanningPhaseEvent
{
    public float duration;
    public PlanningPhaseEvent(float duration) { this.duration = duration; }
}

public class GamePhaseEvent { }