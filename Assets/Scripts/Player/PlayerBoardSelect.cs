using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoardSelect : MonoBehaviour
{
    Subscription<PlayerInteractEvent> playerInteractHandler;
    Subscription<PlayerDisinteractEvent> playerDisInteractHandler;

    HeistBoard interacting;

    void OnEnable()
    {
        playerInteractHandler = EventBus.Subscribe<PlayerInteractEvent>(OnPlayerInteracted);
        playerDisInteractHandler = EventBus.Subscribe<PlayerDisinteractEvent>(OnPlayerDisinteracted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(playerInteractHandler);
        EventBus.Unsubscribe(playerDisInteractHandler);
    }

    void OnPlayerInteracted(PlayerInteractEvent @event)
    {
        var board = @event.target.GetComponent<HeistBoard>();
        if (!board) return;

        if (board.practice) EventBus.Publish(new PlayerPromptEvent(@event.target, InteractionType.StartPractice));
        else EventBus.Publish(new PlayerPromptEvent(@event.target, InteractionType.StartHeist));

        interacting = board;
    }

    void OnPlayerDisinteracted(PlayerDisinteractEvent @event)
    {
        var board = @event.target.GetComponent<HeistBoard>();
        if (!board) return;

        if (board.practice) EventBus.Publish(new PlayerDispromptEvent() { type = InteractionType.StartPractice });
        else EventBus.Publish(new PlayerDispromptEvent() { type = InteractionType.StartHeist });

        interacting = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
        }
    }
}
