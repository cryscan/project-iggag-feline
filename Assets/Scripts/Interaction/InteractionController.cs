using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractEvent
{
    public GameObject subject;
    public Interactable _object;
    public InteractionType type;

    public InteractEvent(GameObject subject, Interactable _object, InteractionType type)
    {
        this.subject = subject;
        this._object = _object;
        this.type = type;
    }
}

public class InteractionController : MonoBehaviour
{
    Interactor[] interactors;

    Subscription<InteractEvent> interactHandler;

    void Awake()
    {
        interactors = GetComponents<Interactor>();
    }

    void OnEnable()
    {
        interactHandler = EventBus.Subscribe<InteractEvent>(OnInteracted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(interactHandler);
    }

    public InteractionType[] GetAvailableInteractions(Interactable interactable)
    {
        var query = from interactor in interactors
                    select interactor.GetInteractions(interactable);
        InteractionType[] interactions = query.Aggregate((result, item) => { return result.Union(item).ToArray(); });
        return interactions;
    }

    void OnInteracted(InteractEvent @event)
    {
        if (@event.subject != gameObject) return;
        foreach (var interactor in interactors)
            interactor.Interact(@event._object, @event.type);
    }
}
