using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationMemory : MonoBehaviour
{
    public Dictionary<GameObject, Vector3> locations { get; private set; } = new Dictionary<GameObject, Vector3>();

    Subscription<InventoryDropEvent> inventoryDropHandler;

    void Start()
    {
        var collectables = GameContext.instance.collectables;
        foreach (var x in collectables)
            locations.Add(x.gameObject, x.transform.position);
    }

    void OnEnable()
    {
        inventoryDropHandler = EventBus.Subscribe<InventoryDropEvent>(OnInventoryDropped);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(inventoryDropHandler);
    }

    public void UpdateLocations(GameObject _object, Vector3 location) => locations[_object] = location;

    void OnInventoryDropped(InventoryDropEvent @event)
    {
        if (@event.subject.gameObject != gameObject) return;
        UpdateLocations(@event._object.gameObject, @event.location);
    }
}
