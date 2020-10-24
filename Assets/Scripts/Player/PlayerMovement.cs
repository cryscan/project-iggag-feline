using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] bool global = true;
    [SerializeField] float speed = 4;
    [SerializeField] float fallout = 10;

    CharacterController controller;
    Vector3 _move;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Vector3 move;
        if (global) move = Vector3.forward * vertical + Vector3.right * horizontal;
        else move = transform.forward * vertical + transform.right * horizontal;

        move = move.normalized;

        _move = Vector3.Lerp(_move, move, 1 - Mathf.Exp(-fallout * Time.deltaTime));
        controller.SimpleMove(_move * speed);
    }
}
