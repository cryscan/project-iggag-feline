using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class PlanCameraController : MonoBehaviour
{
    [SerializeField] float speed = 4;
    [SerializeField] float fallout = 10;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    Vector3 _move = Vector3.zero;
    float size = 10;

    void Start()
    {
        StartCoroutine(ResetCoroutine());
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var target = Mathf.Clamp(size - 10 * Input.mouseScrollDelta.y, 10, 50);
        size = size.FalloutUnscaled(target, fallout);
        virtualCamera.m_Lens.OrthographicSize = size;

        var move = new Vector3(horizontal, 0f, vertical);
        _move = _move.FalloutUnscaled(move, fallout);

        var speed = this.speed * size / 10;
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
