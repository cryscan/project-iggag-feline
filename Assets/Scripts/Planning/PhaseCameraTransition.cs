using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseCameraTransition : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    Camera mainCamera, planCamera;

    Subscription<GameStateChangeEvent> handler;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        mainCamera = Camera.main;
        planCamera = GameObject.FindWithTag("Plan Camera").GetComponent<Camera>();
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
        if (@event.current == GameState.Plan)
        {
            virtualCamera.Priority = 100;
            mainCamera.enabled = false;
            planCamera.enabled = true;
        }
        if (@event.previous == GameState.Plan && @event.current == GameState.Play)
        {
            virtualCamera.Priority = 0;
            mainCamera.enabled = true;
            planCamera.enabled = false;
        }
    }
}
