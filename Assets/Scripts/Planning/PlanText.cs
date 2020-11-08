using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanText : MonoBehaviour
{
    Text text;
    bool planning = false;

    PlaceTrap placeTrap;

    Subscription<GameStateChangeEvent> gameStateChangeHandler;
    Subscription<GameWinEvent> gameWinHandler;

    void Awake()
    {
        text = GetComponent<Text>();
        placeTrap = GameObject.FindGameObjectWithTag("Trap Placer").GetComponent<PlaceTrap>();
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
        /*
        if (planning)
        {
            var timer = GameManager.instance.planTimer;
            var count = placeTrap.count;
            var description = placeTrap.description;
            text.text = $"Planning: {timer.ToString("0.0")} seconds left\nLeft click to put {description} traps ({count} remains)\nPress 1 and 2 to Switch Trap";
        }*/
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
