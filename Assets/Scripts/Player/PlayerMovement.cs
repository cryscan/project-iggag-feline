using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Stand,
    Move,
    Sprint,
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 4;
    public float moveSpeed { get => _moveSpeed; }

    [SerializeField] float _sprintSpeed = 6;
    public float sprintSpeed { get => _sprintSpeed; }

    public float speed { get; private set; }

    [SerializeField] float fallout = 10;

    [SerializeField] float maxSprintTime = 5;
    [SerializeField] LinearProgressBar staminaProgress, recoveryProgress;
    [SerializeField] AudioSource breathAudio;

    bool canSprint = true;
    float stamina, recovery;

    [SerializeField] PlayerState _state;
    public PlayerState state { get => _state; }

    CharacterController controller;
    Vector3 _move;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        speed = _moveSpeed;

        stamina = recovery = maxSprintTime;
        staminaProgress.max = recoveryProgress.max = maxSprintTime;
    }

    void Update()
    {
        UpdateStamina();
        UpdateVelocity();
        UpdateState();
    }

    void UpdateStamina()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        bool hasMoveInput = horizontal != 0 || vertical != 0;

        if (Input.GetKey(KeyCode.LeftShift) && hasMoveInput && canSprint)
        {
            stamina -= Time.deltaTime;
            recovery = stamina;
        }
        else recovery += Time.deltaTime;

        stamina = Mathf.Clamp(stamina, 0, maxSprintTime);
        recovery = Mathf.Clamp(recovery, 0, maxSprintTime);

        canSprint = stamina > 0;
        if (recovery >= maxSprintTime - 0.01) stamina = recovery;

        staminaProgress.current = staminaProgress.current.Fallout(stamina, fallout);
        recoveryProgress.current = recoveryProgress.current.Fallout(recovery, fallout);

        if (!canSprint)
        {
            var volume = (maxSprintTime - recovery) / maxSprintTime;
            breathAudio.volume = breathAudio.volume.Fallout(volume, fallout);
        }
        else breathAudio.volume = 0;
    }

    void UpdateVelocity()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && canSprint) speed = sprintSpeed;
        else speed = moveSpeed;

        var move = transform.forward * vertical + transform.right * horizontal;
        move = move.normalized;

        _move = Vector3.Lerp(_move, move, 1 - Mathf.Exp(-fallout * Time.deltaTime));
        if (controller.enabled) controller.SimpleMove(_move * speed);
    }

    void UpdateState()
    {
        var velocity = controller.velocity;
        var speed = velocity.magnitude;
        if (speed < _moveSpeed / 2) _state = PlayerState.Stand;
        else _state = PlayerState.Move;
    }
}
