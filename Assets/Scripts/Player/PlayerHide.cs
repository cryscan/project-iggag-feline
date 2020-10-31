using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHide : MonoBehaviour
{
	Subscription<PlayerHideEvent> playerHideEventHandler;
	Subscription<PlayerUnhideEvent> playerUnhideEventHandler;
	Subscription<PlayerHideExposeEvent> playerHideExposeEventHandler;
	CharacterController controller;
	private Vector3 position;

	private void Awake()
    {
        playerHideEventHandler = EventBus.Subscribe<PlayerHideEvent>(OnHide);
        playerUnhideEventHandler = EventBus.Subscribe<PlayerUnhideEvent>(OnUnhide);
        playerHideExposeEventHandler = EventBus.Subscribe<PlayerHideExposeEvent>(OnHideExpose);
        controller = GetComponent<CharacterController>();
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(playerHideEventHandler);
        EventBus.Unsubscribe(playerUnhideEventHandler);
        EventBus.Unsubscribe(playerHideExposeEventHandler);
    }

    void OnHide(PlayerHideEvent @event)
    {
    	position = @event.position;
    	controller.enabled = false;
    	transform.position = position; 
    }

    void OnUnhide(PlayerUnhideEvent @event)
    {
        controller.enabled = true;
    }

    void OnHideExpose(PlayerHideExposeEvent @event)
    {
    	Debug.Log("TODO: OnHideExpose handler for PlayerHideExposeEvents.");
    }
}
