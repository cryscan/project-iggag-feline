﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeIndicator : MonoBehaviour
{
    public Vector3 center;
    public float radius = 0;
    [SerializeField] int segments = 32;
    [SerializeField] float fallout = 10;

    Camera _camera, UICamera;
    float _radius = 0;
    Vector3[] points;

    LineRenderer lineRenderer;

    void Awake()
    {
        _camera = Camera.main;
        UICamera = GameObject.FindWithTag("UI Camera").GetComponent<Camera>();

        points = new Vector3[segments + 1];

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Length;
    }

    void Update()
    {
        _radius = _radius.FalloutUnscaled(radius, fallout);
        UpdatePoints();
    }

    void UpdatePoints()
    {
        for (int i = 0; i < points.Length; ++i)
        {
            var angle = 2 * Mathf.PI * i / segments;
            var position = center + _radius * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            points[i] = UICamera.ViewportToWorldPoint(_camera.WorldToViewportPoint(position));
        }
        lineRenderer.SetPositions(points);
    }
}
