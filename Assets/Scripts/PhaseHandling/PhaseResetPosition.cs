using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseResetPosition : MonoBehaviour
{
    [SerializeField] Vector3 position;

    void Awake()
    {
        EventBus.Subscribe<GamePhaseEvent>(_OnGamePhase);
    }

    void _OnGamePhase(GamePhaseEvent e)
    {
    	Debug.Log(transform.position);
        GetComponent<CharacterController>().Move(position - transform.position);
    	Debug.Log(transform.position);
    }
}
