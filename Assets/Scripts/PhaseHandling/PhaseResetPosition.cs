using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseResetPosition : MonoBehaviour
{
	Subscription<GamePhaseEvent> game_phase_event_subscription;
	[SerializeField] Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        game_phase_event_subscription = EventBus.Subscribe<GamePhaseEvent>(_OnGamePhase);
    }

    void _OnGamePhase(GamePhaseEvent e)
    {
    	Debug.Log(transform.position);
        GetComponent<CharacterController>().Move(position - transform.position);
    	Debug.Log(transform.position);
    }
}
