using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakable : MonoBehaviour
{
    [SerializeField] protected Role[] roles;
    [SerializeField] protected Breakable[] breakables;

    [SerializeField] GameObject effect;
    public bool broken { get; private set; } = false;

    protected virtual void Start()
    {
        effect?.SetActive(false);
        foreach (var role in roles) role.enabled = false;
    }

    protected virtual void Update()
    {
        effect?.SetActive(broken);
    }

    public virtual void Break()
    {
        broken = true;
        foreach (var role in roles) role.enabled = true;
        foreach (var breakable in breakables) breakable.Break();
    }

    public virtual void Repair()
    {
        broken = false;
        foreach (var role in roles) role.enabled = false;
        foreach (var breakable in breakables) breakable.Repair();
    }
}
