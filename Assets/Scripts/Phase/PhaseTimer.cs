using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseTimer : MonoBehaviour
{
    Text text;
    bool planning = false;

    Subscription<GameStateChangeEvent> handler;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void OnEnable()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
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
}
