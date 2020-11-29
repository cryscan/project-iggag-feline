using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Carryable))]
public class CarryRole : Role
{
    public Transform destination;
    public Carryable carryable { get; private set; }

    void Awake()
    {
        carryable = GetComponent<Carryable>();
    }
}
