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

    public TrapEvent(GameObject target, TrapType type)
    {
        this.target = target;
        this.type = type;
    }
}

public class TrapHandler : MonoBehaviour
{
    [SerializeField] TrapType trapType;

    private void OnTriggerEnter(Collider other)
    {
        GameObject _object = other.gameObject;
        if (_object.CompareTag("Enemy"))
        {
            EventBus.Publish(new TrapEvent(_object, trapType));
        }        
    }
}
