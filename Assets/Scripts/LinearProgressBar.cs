using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearProgressBar : MonoBehaviour
{
    [SerializeField] Image mask;
    public float max;
    public float current;
    public float amount { get => current / max; }

    public Vector3 start { get; private set; }
    public Vector3 end { get; private set; }

    Camera _camera;
    Canvas canvas;

    void Awake()
    {
        _camera = Camera.main;
        canvas = GetComponentInParent<Canvas>();

        CalculatePoints();
    }

    void Update()
    {
        mask.fillAmount = amount;
    }

    void LateUpdate()
    {
        CalculatePoints();
    }

    void CalculatePoints()
    {
        var position = mask.transform.position;
        var width = mask.rectTransform.rect.width;
        width = width * canvas.scaleFactor;
        // start = _camera.ScreenToViewportPoint(new Vector3(position.x - width / 2, position.y, 0));
        // end = _camera.ScreenToViewportPoint(new Vector3(position.x + width / 2, position.y, 0));
        start = _camera.ScreenToViewportPoint(new Vector3(position.x - width / 2, position.y, 0));
        end = _camera.ScreenToViewportPoint(new Vector3(position.x + width / 2, position.y, 0));
    }

    public Vector3 GetMiddlePoint(float amount)
    {
        return _camera.ViewportToScreenPoint(Vector3.Lerp(start, end, amount));
    }
}
