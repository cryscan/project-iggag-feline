using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CollectableType
{
    KeyCard,
    Handgun,
    Crate,
}

[RequireComponent(typeof(Interactable))]
public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType _type;
    public CollectableType type { get => _type; }

    public Inventory subject { get; private set; }

    Rigidbody rb;
    Collider[] colliders;

    Subscription<CollectEvent> collectHandler;
    Subscription<DropEvent> dropHandler;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        var interactable = GetComponent<Interactable>();
        interactable.Rigister(InteractionType.Collect);
        interactable.Rigister(InteractionType.Drop);
    }

    void OnEnable()
    {
        collectHandler = EventBus.Subscribe<CollectEvent>(OnCollected);
        dropHandler = EventBus.Subscribe<DropEvent>(OnDropped);
    }

    void OnCollected(CollectEvent @event)
    {
        if (@event._object != this) return;

        subject = @event.subject;

        if (rb) rb.isKinematic = true;
        SetEnabledColliders(false);
    }

    void OnDropped(DropEvent @event)
    {
        if (@event._object != this) return;

        subject = null;

        if (rb) rb.isKinematic = false;
        SetEnabledColliders(true);
    }

    void SetEnabledColliders(bool enabled) { foreach (var collider in colliders) collider.enabled = enabled; }
}