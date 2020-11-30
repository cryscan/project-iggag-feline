using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanCameraController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float fallout = 10;

    Vector3 _move = Vector3.zero;
    float size = 10;

    Camera planCamera;

    void Awake()
    {
        planCamera = GameObject.FindWithTag("Plan Camera").GetComponent<Camera>();
    }

    void Start()
    {
        StartCoroutine(ResetCoroutine());
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (planCamera)
        {
            var zoom = Input.mouseScrollDelta.y;
            var target = Mathf.Clamp(size + zoom, 10, 50);
            size = size.FalloutUnscaled(target, fallout);
            planCamera.orthographicSize = size;
        }

        var move = new Vector3(horizontal, 0f, vertical);
        _move = _move.FalloutUnscaled(move, fallout);

        transform.position = transform.position + _move * speed * Time.unscaledDeltaTime;
    }

    IEnumerator ResetCoroutine()
    {
        yield return null;

        var player = GameObject.FindWithTag("Player");
        var position = player.transform.position;
        position.y = transform.position.y;
        transform.position = position;
    }
}
