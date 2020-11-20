using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    [SerializeField] float comeOutDistance = 2;

    Hideable interacting;
    Hideable hiding;

    Subscription<PlayerInteractEvent> playerInteractHandler;
    Subscription<PlayerDisinteractEvent> playerDisInteractHandler;

    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hiding)
            {
                StopAllCoroutines();
                controller.enabled = false;
                transform.position = hiding.transform.position + transform.forward * comeOutDistance;
                controller.enabled = true;

                hiding.ComeOut();
                hiding = null;
            }
            else if (interacting)
            {
                hiding = interacting;
                hiding.Hide(gameObject);
                StartCoroutine(HideCoroutine());
            }
        }
    }

    void OnPlayerInteracted(PlayerInteractEvent @event)
    {
        var hidable = @event.target.GetComponent<Hideable>();
        if (hidable && !hidable.inside)
        {
            interacting = hidable;
            EventBus.Publish(new PlayerPromptEvent(@event.target, InteractionType.Hide));
        }
    }

    void OnPlayerDisinteracted(PlayerDisinteractEvent @event)
    {
        var hidable = @event.target.GetComponent<Hideable>();
        if (hidable)
        {
            interacting = null;
            EventBus.Publish(new PlayerDispromptEvent() { type = InteractionType.Hide });
        }
    }

    IEnumerator HideCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        controller.enabled = false;
        // transform.position = new Vector3(1000, 0, 1000);
    }
}
