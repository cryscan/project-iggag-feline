using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseTimer : MonoBehaviour
{
	Subscription<PlanningPhaseEvent> planning_phase_event_subscription;
	Subscription<GamePhaseEvent> game_phase_event_subscription;
	private Text text;
	private float timer;
	private bool started;

    // Start is called before the first frame update
    void Start()
    {
    	text = this.GetComponent<Text>();
    	started = false;
        planning_phase_event_subscription = EventBus.Subscribe<PlanningPhaseEvent>(_OnPlanningPhase);
        game_phase_event_subscription = EventBus.Subscribe<GamePhaseEvent>(_OnGamePhase);
    }

    void Update()
    {
    	if (started) {
    		text.text = "Planning Phase: " + timer + " seconds left";
    		if (timer > 0.0f) {
    			timer -= Time.deltaTime;
    		}
    		else {
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
