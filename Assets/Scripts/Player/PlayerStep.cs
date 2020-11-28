using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepEvent : IEvent
{
    public Vector3 position;
    public Vector3 velocity;

    public PlayerStepEvent(Vector3 position, Vector3 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
}

public class PlayerStep : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] Transform pivot;
    [SerializeField] Transform head;
    [SerializeField] float amplitude = 0.2f;
    [SerializeField] float period = 0.5f;
    [SerializeField] float fallout = 10;

    [Header("Sound")]
    [SerializeField] GameObject stepAudio;

    CharacterController controller;
    PlayerMovement movement;
    AudioSource[] audios;

    float blend = 0;
    bool stepped = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
        audios = stepAudio.GetComponents<AudioSource>();
    }

    void Update()
    {
        var velocity = controller.velocity;
        velocity.y = 0;
        var speed = velocity.magnitude;

        float target = Mathf.Clamp01(speed / movement.moveSpeed);
        blend = blend.Fallout(target, fallout);

        var position = head.position;
        position.y = pivot.position.y + Oscillation();
        head.position = position;
    }

    float Oscillation()
    {
        var frequency = 2 * Mathf.PI / period;
        var oscillation = Mathf.Sin(frequency * Time.time);

        if (oscillation < -0.8f && !stepped)
        {
            var rand = Random.Range(0, audios.Length);
            var audio = audios[rand];
            audio.volume = blend;
            audio.volume *= Random.Range(0.8f, 1.0f);
            audio.pitch = Random.Range(0.8f, 1.2f);
            audio.Play();

            EventBus.Publish(new PlayerStepEvent(transform.position, controller.velocity));
            stepped = true;
        }
        if (oscillation > -0.8f) stepped = false;

        return blend * amplitude * oscillation;
    }
}
