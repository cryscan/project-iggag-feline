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
    [SerializeField] float normalSpeed = 4;
    [SerializeField] float sprintSpeed = 8;
    [SerializeField] float fallout = 10;

    [SerializeField] PlayerState _state;
    public PlayerState state { get => _state; }

    CharacterController controller;
    float speed;
    Vector3 _move;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        speed = normalSpeed;
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)) speed = sprintSpeed;
        else speed = normalSpeed;

        var move = transform.forward * vertical + transform.right * horizontal;
        move = move.normalized;

        _move = Vector3.Lerp(_move, move, 1 - Mathf.Exp(-fallout * Time.deltaTime));
        var velocity = _move * speed;
        controller.SimpleMove(velocity);

        var realSpeed = velocity.magnitude;
        if (realSpeed < normalSpeed / 2) _state = PlayerState.Stand;
        else if (realSpeed < (normalSpeed + sprintSpeed) / 2) _state = PlayerState.Move;
        else _state = PlayerState.Sprint;
    }
}
