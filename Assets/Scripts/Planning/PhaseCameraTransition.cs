using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCameraTransition : MonoBehaviour
{
    CinemachineVirtualCamera _camera;

    Subscription<GameStateChangeEvent> handler;

    void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
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
        if (@event.current == GameState.Plan) _camera.Priority = 100;
        if (@event.previous == GameState.Plan && @event.current == GameState.Play) _camera.Priority = 0;
    }
}
