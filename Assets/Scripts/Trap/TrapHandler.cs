using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    Frozen,
    Eliminate
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

    private void OnTriggerEnter(Collider other)
    {
        GameObject _object = other.gameObject;
        Debug.Log($"[Trap] {_object}");

        if (_object.CompareTag("Enemy"))
        {
            EventBus.Publish(new TrapEvent(_object, trapType, duration));
            Destroy(gameObject);
        }
    }
}
