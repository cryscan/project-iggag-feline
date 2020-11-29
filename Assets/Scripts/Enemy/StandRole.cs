using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StandRole : Role
{
    public List<GameObject> points { get; private set; }

    void Awake()
    {
        var query = from i in Enumerable.Range(0, transform.childCount)
                    select transform.GetChild(i).gameObject;
        points = query.ToList();
    }
}
