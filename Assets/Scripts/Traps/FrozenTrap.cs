using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenTrap : TrapBase
{
    [SerializeField] float _range = 5;
    public float range { get => _range; }

    [SerializeField] float _duration = 5;
    public float duration { get => _duration; }
}
