using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class GuardTrapReaction : MonoBehaviour
{
    BehaviorTree behavior;

    Subscription<TrapEvent> trapEventHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
    }

    private void OnEnable()
    {
        trapEventHandler = EventBus.Subscribe<TrapEvent>(OnTrap);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(trapEventHandler);
    }

    void OnTrap(TrapEvent @event)
    {
        if (@event.target == this.gameObject)
        {
            switch (@event.type)
            {
                case TrapType.Frozen:
                    Frozen(@event.duration);
                    break;
                case TrapType.Eliminate:
                    break;
            }
        }
    }

    void Frozen(float duration)
    {
        behavior.SendEvent<object>("Frozen", duration);
    }
}
