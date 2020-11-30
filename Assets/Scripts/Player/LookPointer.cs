using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPointer : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    Camera _camera;

    void Awake()
    {
        _camera = GameObject.FindWithTag("Plan Camera").GetComponent<Camera>();
    }

    void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundLayers))
            transform.position = hit.point;
    }
}
