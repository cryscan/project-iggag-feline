using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicBounds : MonoBehaviour
{
    public Bounds bounds { get => GetBounds(); }
    Bounds _bounds;

    bool updated = false;
    Collider[] colliders;

    void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    void LateUpdate()
    {
        updated = false;
    }

    Bounds GetBounds()
    {
        if (!updated)
        {
            _bounds = new Bounds(transform.position, Vector3.zero);
            foreach (var collider in colliders) _bounds.Encapsulate(collider.bounds);
            updated = true;
        }
        return _bounds;
    }
}
