using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTrapReaction : MonoBehaviour
{
    Subscription<TrapEvent> trapEventHandler;

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
            switch(@event.type)
            {
                case TrapType.Frozen:
                    break;
                case TrapType.Eliminate:
                    break;
            }
        }
    }

    void Frozen()
    {

    }
}
