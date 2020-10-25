using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseTimer : MonoBehaviour
{
    Text text;
    float timer;
    bool started;

    void Awake()
    {
        text = GetComponent<Text>();
        started = false;

        EventBus.Subscribe<PlanningPhaseEvent>(_OnPlanningPhase);
        EventBus.Subscribe<GamePhaseEvent>(_OnGamePhase);
    }

    void Update()
    {
        if (started)
        {
            text.text = "Planning Phase: " + timer + " seconds left";
            if (timer > 0.0f) timer -= Time.deltaTime;
            else
            {
                started = false;
                text.text = "Planning Phase: 0.0 seconds left";
            }
        }
    }

    void _OnPlanningPhase(PlanningPhaseEvent e)
    {
        timer = e.duration;
        started = true;
    }

    void _OnGamePhase(GamePhaseEvent e)
    {
        text.text = "Game Phase has begun!";
    }
}
