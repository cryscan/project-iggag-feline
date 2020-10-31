using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanTimer : MonoBehaviour
{
    Text text;
    bool planning = false;

    Subscription<GameStateChangeEvent> handler;
    Subscription<PlayerHideEvent> playerHideEventHandler;
    Subscription<PlayerUnhideEvent> playerUnhideEventHandler;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void OnEnable()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
        playerHideEventHandler = EventBus.Subscribe<PlayerHideEvent>(OnHide);
        playerUnhideEventHandler = EventBus.Subscribe<PlayerUnhideEvent>(OnUnhide);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(handler);
    }

    void Update()
    {
        if (planning)
        {
            var timer = GameManager.instance.planTimer;
            var trapCounter = GameManager.instance.trapCounter;
            text.text = $"Planning: {timer.ToString("0.0")} seconds left\n Left click to put frozen traps ({trapCounter} remains)";
        }
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        switch (@event.current)
        {
            case GameState.Plan:
                planning = true;
                break;
            case GameState.Play:
                planning = false;
                text.text = "Avoid being caught and escape";
                break;
            default:
                planning = false;
                text.text = "";
                break;
        }
    }

    void OnHide(PlayerHideEvent @event)
    {
        text.text = "Press space to leave the crate"; 
    }

    void OnUnhide(PlayerUnhideEvent @event)
    {
        text.text = "Avoid being caught and escape";
    }
}
