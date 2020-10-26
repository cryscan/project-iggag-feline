using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseResetPosition : MonoBehaviour
{
    [SerializeField] GameState state;
    [SerializeField] Vector3 position;

    CharacterController controller;

    Subscription<GameStateChangeEvent> handler;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.current != state) return;

        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }
}
