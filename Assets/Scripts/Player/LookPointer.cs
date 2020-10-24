using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPointer : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundLayers))
            transform.position = hit.point;
    }
}
