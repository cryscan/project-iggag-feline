using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCameraTransition : MonoBehaviour
{
    CinemachineVirtualCamera _camera;

    Subscription<GameStateChangeEvent> handle;

    void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }

    void OnEnable()
    {
        handle = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handle);
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.current == GameState.Plan) _camera.Priority = 100;
        if (@event.previous == GameState.Plan && @event.current == GameState.Play) _camera.Priority = 0;
    }
}
