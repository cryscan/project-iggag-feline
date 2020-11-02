using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ControlPromptLister))]
public class ControlHints : MonoBehaviour
{
    ControlPromptLister lister;
    Camera _camera;

    GameObject player;
    InteractionController controller;

    DynamicBounds bounds;

    Subscription<PlayerPromptEvent> playerPromptHandler;
    Subscription<PlayerUnfocusEvent> playerUnfocusHandler;

    void Awake()
    {
        _camera = Camera.main;
        lister = GetComponent<ControlPromptLister>();

        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<InteractionController>();
    }

    void OnEnable()
    {
        playerPromptHandler = EventBus.Subscribe<PlayerPromptEvent>(OnPlayerPrompted);
        playerUnfocusHandler = EventBus.Subscribe<PlayerUnfocusEvent>(OnPlayerUnfocused);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(playerPromptHandler);
        EventBus.Unsubscribe(playerUnfocusHandler);
    }

    void Start()
    {
        lister.SetActiveAll(false);
    }

    void Update()
    {
        Relocate();
    }

    void Relocate()
    {
        if (bounds)
            transform.position = _camera.WorldToScreenPoint(bounds.bounds.center);
    }

    void OnPlayerPrompted(PlayerPromptEvent @event)
    {
        lister.SetActiveAll(false);

        var interactions = controller.GetAvailableInteractions(@event._object);
        foreach (var interaction in interactions)
            lister.Find(interaction)?.SetActive(true);

        bounds = @event._object.GetComponent<DynamicBounds>();
        Relocate();
    }

    void OnPlayerUnfocused(PlayerUnfocusEvent @event)
    {
        lister.SetActiveAll(false);
        bounds = null;
    }
}
