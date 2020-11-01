using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    [SerializeField] LayerMask collectableLayers;
    [SerializeField] float radius = 0.1f;
    public int priority { get; private set; } = 0;

    void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, collectableLayers, QueryTriggerInteraction.Collide);
        priority = colliders.Length;
    }
}
