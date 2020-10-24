using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

        var move = transform.forward * vertical + transform.right * horizontal;
        move = move.normalized;

        _move = Vector3.Lerp(_move, move, 1 - Mathf.Exp(-fallout * Time.deltaTime));
        controller.SimpleMove(_move * speed);
    }
}
