using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShakeController : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float spacial = 0;
    [SerializeField] float multiplier = 20;

    [SerializeField] float impulse = 5;
    [SerializeField] float fallout = 10;

    float amplitude;

    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin perlin;

    Subscription<TrapActivateEvent> trapActivateHandler;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void OnEnable()
    {
        trapActivateHandler = EventBus.Subscribe<TrapActivateEvent>(OnTrapActivated);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe(trapActivateHandler);
    }

    void Update()
    {
        amplitude = amplitude.Fallout(0, 10);
        perlin.m_AmplitudeGain = amplitude;
    }

    void OnTrapActivated(TrapActivateEvent @event)
    {
        float planerImpulse = impulse;

        var position = @event.trap.transform.position;
        var distance = Vector3.SqrMagnitude(transform.position - position);
        float spacialImpulse = impulse / distance;

        amplitude += Mathf.Lerp(planerImpulse, spacialImpulse, spacial);
    }
}
