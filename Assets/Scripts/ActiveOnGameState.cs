using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveOnGameState : MonoBehaviour
{
    [SerializeField] GameState[] states;
    [SerializeField] GameObject[] objects;

    Subscription<GameStateChangeEvent> handler;

    void Awake()
    {
        handler = EventBus.Subscribe<GameStateChangeEvent>(OnGameStateChanged);
    }

    void OnDestroy()
    {
        EventBus.Unsubscribe(handler);
    }

    void OnGameStateChanged(GameStateChangeEvent @event)
    {
        var active = states.Contains(@event.current);
        foreach (var _object in objects) _object.SetActive(active);
    }
}
