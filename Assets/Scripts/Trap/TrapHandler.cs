using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    Frozen,
    Eliminate,
    Distraction
}

public class TrapEvent
{
    public GameObject target;
    public TrapType type;
    public float duration;

    public TrapEvent(GameObject target, TrapType type, float duration)
    {
        this.target = target;
        this.type = type;
        this.duration = duration;
    }
}

public class TrapHandler : MonoBehaviour
{
    [SerializeField] TrapType trapType;
    [SerializeField] float duration = 3;
    [SerializeField] bool active = false;

    private void OnEnable()//public void Activate()
    {
        active = true;
        if (trapType == TrapType.Distraction)
        {
            EventBus.Publish(new TrapEvent(this.gameObject, trapType, duration));
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            GameObject _object = other.gameObject;
            Debug.Log($"[Trap] {_object}");

            if (_object.CompareTag("Enemy"))
            {
                if (trapType == TrapType.Frozen)
                {
                    EventBus.Publish(new TrapEvent(_object, trapType, duration));
                }
                Destroy(gameObject);
            }
        }
    }
}
