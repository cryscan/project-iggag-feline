using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class GuardTrapReaction : MonoBehaviour
{
    BehaviorTree behavior;
    GuardReaction reaction;
    Subscription<TrapEvent> trapEventHandler;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
        reaction = GetComponent<GuardReaction>();
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
        else if (@event.type == TrapType.Distraction)
        {
            Distraction(@event.target);
        }
    }

    void Frozen(float duration)
    {
        behavior.SendEvent<object>("Frozen", duration);
    }

    void Distraction(GameObject location)
    {
        reaction.Alert();
        behavior.SendEvent<object>("Alert", location.transform.position);
    }
}
