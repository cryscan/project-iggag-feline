using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class GuardReaction : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] Color dealertedLightColor = Color.white;
    [SerializeField] Color alertedLightColor = Color.red;

    BehaviorTree behavior;

    Subscription<DetectEvent> detectEventHandler;

    bool alerted = false;

    void Awake()
    {
        behavior = GetComponent<BehaviorTree>();
    }

    void OnEnable()
    {
        detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
        behavior.RegisterEvent("Dealert", OnDealerted);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(detectEventHandler);
        behavior.UnregisterEvent("Dealert", OnDealerted);
    }

    void OnDetected(DetectEvent @event)
    {
        if (@event.type == DetectionType.Guard && @event.subject != gameObject) return;

        Alert();
        behavior.SendEvent<object>("Alert", @event.spotPoint);
    }

    void OnDealerted() => Dealert();

    void Alert()
    {
        alerted = true;
        _light.color = alertedLightColor;
    }

    void Dealert()
    {
        if (!alerted) Debug.LogError("[Guard] makes no sense dealert when not alerted.");

        alerted = false;
        _light.color = dealertedLightColor;
    }
}