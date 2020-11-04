using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanCameraController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float fallout = 10;

    Vector3 _move = Vector3.zero;

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var move = new Vector3(horizontal, 0f, vertical);
        _move = _move.Fallout(move, fallout);

        transform.position = transform.position + _move * speed * Time.deltaTime;
    }
}
