using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPoint : Role
{
    [SerializeField] Breakable _breakable;
    public Breakable breakable { get => _breakable; }
}
