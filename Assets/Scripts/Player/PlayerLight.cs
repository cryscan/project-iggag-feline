using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerLight : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] float standIntensity = 1;
    [SerializeField] float moveIntensity = 2;
    [SerializeField] float sprintIntensity = 4;
    [SerializeField] float fallout = 10;

    float _intensity;

    PlayerMovement movement;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        var state = movement.state;
        float intensity = 0;
        switch (state)
        {
            case PlayerState.Stand:
                intensity = standIntensity;
                break;
            case PlayerState.Move:
                intensity = moveIntensity;
                break;
            case PlayerState.Sprint:
                intensity = sprintIntensity;
                break;
        }

        _intensity = _intensity.Fallout(intensity, fallout);
        _light.intensity = _intensity;
    }
}
