using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepEvent
{
    public float blend;
    public Vector3 position;

    public PlayerStepEvent(float blend, Vector3 position)
    {
        this.blend = blend;
        this.position = position;
    }
}

public class PlayerOscillation : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] float amplitude = 0.2f;
    [SerializeField] float period = 0.5f;
    [SerializeField] float fallout = 10;

    CharacterController controller;
    PlayerMovement movement;

    float height;
    float blend = 0;

    bool stepped = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();

        height = head.position.y;
    }

    void Update()
    {
        var velocity = controller.velocity;
        velocity.y = 0;
        var speed = velocity.magnitude;

        float target = speed / movement.moveSpeed;
        blend = blend.Fallout(target, fallout);

        var position = head.position;
        position.y = height + blend * Oscillation();
        head.position = position;
    }

    float Oscillation()
    {
        var frequency = 2 * Mathf.PI / period;
        var oscillation = Mathf.Sin(frequency * Time.time);

        if (oscillation < -0.8f && !stepped)
        {
            EventBus.Publish(new PlayerStepEvent(blend, transform.position));
            stepped = true;
        }
        if (oscillation > -0.8f) stepped = false;

        return amplitude * oscillation;
    }
}
