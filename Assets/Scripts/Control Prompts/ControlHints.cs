using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ControlPromptLister))]
public class ControlHints : MonoBehaviour
{
    ControlPromptLister lister;
    Camera _camera;

    DynamicBounds bounds;

    Subscription<PlayerPromptEvent> playerPromptHandler;
    Subscription<PlayerDispromptEvent> playerDispromptHandler;

    void Awake()
    {
        _camera = Camera.main;
        lister = GetComponent<ControlPromptLister>();
    }

    void OnEnable()
    {
        playerPromptHandler = EventBus.Subscribe<PlayerPromptEvent>(OnPlayerPrompted);
        playerDispromptHandler = EventBus.Subscribe<PlayerDispromptEvent>(OnPlayerDisprompted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(playerPromptHandler);
        EventBus.Unsubscribe(playerDispromptHandler);
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
        /*
        lister.SetActiveAll(false);
        if (!@event._object) return;

        var interactions = controller.GetAvailableInteractions(@event._object);
        foreach (var interaction in interactions)
            lister.Find(interaction)?.SetActive(true);

        bounds = @event._object.GetComponent<DynamicBounds>();
        Relocate();
        */
        if (!@event.target) return;

        lister.Find(@event.type)?.SetActive(true);
        bounds = @event.target.GetComponent<DynamicBounds>();
        Relocate();
    }

    void OnPlayerDisprompted(PlayerDispromptEvent @event)
    {
        /*
        lister.SetActiveAll(false);
        bounds = null;
        */
        lister.Find(@event.type)?.SetActive(false);
    }
}
