using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicBounds))]
public class Interactable : MonoBehaviour
{
    public DynamicBounds bounds { get; private set; }
    public List<InteractionType> interactions { get; private set; }

    void Awake()
    {
        bounds = GetComponent<DynamicBounds>();
    }

    public void Rigister(InteractionType type) => interactions.Add(type);
    public bool Contains(InteractionType type) => interactions.Contains(type);
}
