﻿using System.Collections;
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

    [SerializeField] PlayerState _state;
    public PlayerState state { get => _state; }

    CharacterController controller;
    Vector3 _move;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        speed = _moveSpeed;
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift)) speed = sprintSpeed;
        else speed = moveSpeed;

        var move = transform.forward * vertical + transform.right * horizontal;
        move = move.normalized;

        _move = Vector3.Lerp(_move, move, 1 - Mathf.Exp(-fallout * Time.deltaTime));
        if (controller.enabled) controller.SimpleMove(_move * speed);

        UpdateState();
    }

    void UpdateState()
    {
        var velocity = controller.velocity;
        var speed = velocity.magnitude;
        if (speed < _moveSpeed / 2) _state = PlayerState.Stand;
        else _state = PlayerState.Move;
    }
}
