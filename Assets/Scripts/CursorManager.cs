using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    Subscription<GameStateChangeEvent> handler;

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
        if (@event.current == GameState.Play)
        {
            Debug.Log($"[Cursor] locked at {@event.current}");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Debug.Log($"[Cursor] released at {@event.current}");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
