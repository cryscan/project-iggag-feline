using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionTrap : TrapBase
{
    [SerializeField] float _range = 10;
    public float range { get => _range; }

    [SerializeField] int maxCount = 2;
    int count = 0;

    public void IncreaseCount() => ++count;

    public bool ReachedMaxCount() => count >= maxCount;
}
