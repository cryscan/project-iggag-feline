using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReaction : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] Color dealertedLightColor = Color.white;
    [SerializeField] Color alertedLightColor = Color.red;
    [SerializeField] float alertedTime = 1;

    bool alerted = false;
    float currentAltertedTime = 0;

    Subscription<DetectEvent> detectEventHandler;

    void OnEnable()
    {
        detectEventHandler = EventBus.Subscribe<DetectEvent>(OnDetected);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(detectEventHandler);
    }

    void Update()
    {
        if (currentAltertedTime > 0)
            currentAltertedTime -= Time.deltaTime;
        else if (alerted)
        {
            alerted = false;
            _light.color = dealertedLightColor;
        }
    }

    void OnDetected(DetectEvent @event)
    {
        if (@event.subject != gameObject) return;

        alerted = true;
        _light.color = alertedLightColor;
        currentAltertedTime = alertedTime;
    }
}
