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

    Resolution resolution;

    void Awake()
    {
        CalculatePoints();
        resolution = Screen.currentResolution;
    }

    void Update()
    {
        mask.fillAmount = amount;

        {
            var current = Screen.currentResolution;
            if (current.width != resolution.width || current.height != resolution.height)
            {
                resolution = current;
                CalculatePoints();
            }
        }
    }

    void CalculatePoints()
    {
        var position = mask.transform.position;
        position.y -= mask.rectTransform.rect.height * mask.rectTransform.pivot.y;

        var width = mask.rectTransform.rect.width;
        start = new Vector3(position.x - width / 2, position.y, 0);
        end = new Vector3(position.x + width / 2, position.y, 0);
    }
}
