using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanTimer : MonoBehaviour
{
    Text text;
    bool planning = false;

    Subscription<GameStateChangeEvent> gameStateChangeHandler;
    Subscription<GameWinEvent> gameWinHandler;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void OnEnable()
    {
        gameStateChangeHandler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
        gameWinHandler = EventBus.Subscribe<GameWinEvent>(OnGameWon);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(gameStateChangeHandler);
        EventBus.Unsubscribe(gameWinHandler);
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

    void OnGameWon(GameWinEvent @event)
    {
        text.text = "You Completed the Level!";
    }
}
