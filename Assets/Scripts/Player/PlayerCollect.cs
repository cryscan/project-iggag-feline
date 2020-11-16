using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    Subscription<PlayerInteractEvent> playerInteractHandler;
    Subscription<PlayerDisinteractEvent> playerDisInteractHandler;

    GameObject document;
    public GameObject door;

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
        if (@event.target.CompareTag("Document"))
        {
            document = @event.target;
            EventBus.Publish(new PlayerPromptEvent(@event.target, InteractionType.PickUp));
        }
    }

    void OnPlayerDisinteracted(PlayerDisinteractEvent @event)
    {
        if (@event.target.CompareTag("Document"))
        {
            document = null;
            EventBus.Publish(new PlayerDispromptEvent() { type = InteractionType.PickUp });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (document)
            {
                document.SetActive(false);
                door.GetComponent<GameDoor>().condition = true;
            }
        }
    }
}
