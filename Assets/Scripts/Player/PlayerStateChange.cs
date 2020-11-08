using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateChange : MonoBehaviour
{
    Subscription<GameStateChangeEvent> handler;

    PlayerLook playerLook;
    PlayerMovement playerMovement;
    CharacterController controller;

    private void Awake()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        if (@event.current == GameState.Plan)
        {
            playerLook.enabled = false;
            playerMovement.enabled = false;
            controller.enabled = false;
        }

        if (@event.current == GameState.Play && @event.previous == GameState.Plan)
        {
            playerLook.enabled = true;
            playerMovement.enabled = true;
            controller.enabled = true;
        }
    }
}
