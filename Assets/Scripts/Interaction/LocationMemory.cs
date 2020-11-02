using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationMemory : MonoBehaviour
{
    public Dictionary<GameObject, Vector3> locations { get; private set; } = new Dictionary<GameObject, Vector3>();

    Subscription<DropEvent> dropHandler;

    void Awake()
    {
        var collectables = FindObjectsOfType<Collectable>();
        foreach (var x in collectables)
            locations.Add(x.gameObject, x.transform.position);
    }

    void OnEnable()
    {
        dropHandler = EventBus.Subscribe<DropEvent>(OnInventoryDropped);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(dropHandler);
    }

    public void UpdateLocations(GameObject _object, Vector3 location) => locations[_object] = location;

    void OnInventoryDropped(DropEvent @event)
    {
        if (@event.subject.gameObject != gameObject) return;
        UpdateLocations(@event._object.gameObject, @event.location);
    }
}
