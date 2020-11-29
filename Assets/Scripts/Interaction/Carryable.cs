using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    [SerializeField] float fallout = 10;
    public bool carrying { get; private set; } = false;

    Rigidbody rb;
    Collider _collider;

    Transform holder = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (holder)
        {
            var position = holder.position;
            transform.position = transform.position.Fallout(position, fallout);

            var rotation = holder.rotation;
            transform.rotation = transform.rotation.Fallout(rotation, fallout);
        }
    }

    public bool Carry(Transform holder)
    {
        if (carrying) return false;

        this.holder = holder;

        rb.isKinematic = true;
        _collider.enabled = false;

        carrying = true;
        return true;
    }

    public void Drop()
    {
        if (!carrying) return;

        holder = null;

        rb.isKinematic = false;
        _collider.enabled = true;

        carrying = false;
    }
}
