using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakable : MonoBehaviour
{
    [SerializeField] Role[] roles;

    [SerializeField] GameObject effect;
    public bool broken { get; private set; } = false;

    protected virtual void Update()
    {
        effect?.SetActive(broken);
    }

    public virtual void Break()
    {
        broken = true;
        foreach (var role in roles) role.SetValid(true);
    }

    public virtual void Repair()
    {
        broken = false;
        foreach (var role in roles) role.SetValid(false);
    }
}
