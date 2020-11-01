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

public class Collectable : MonoBehaviour
{
    [SerializeField] CollectableType _type;
    public CollectableType type { get => _type; }

    [SerializeField] float dropRange = 2;
    [SerializeField] float dropFallout = 10;

    public Inventory subject { get; private set; }

    Rigidbody rb;
    Collider[] colliders;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public void Collect(Inventory inventory)
    {
        subject = inventory;

        if (rb) rb.isKinematic = true;
        SetEnabledColliders(false);
    }

    public void Drop()
    {
        subject = null;

        if (rb) rb.isKinematic = false;
        SetEnabledColliders(true);
    }

    void SetEnabledColliders(bool enabled) { foreach (var collider in colliders) collider.enabled = enabled; }
}