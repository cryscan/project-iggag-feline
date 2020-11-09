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

    public Vector3 startPoint { get; private set; }
    public Vector3 endPoint { get; private set; }

    void Awake()
    {
        var position = mask.transform.position;
        var width = mask.rectTransform.rect.width;
        startPoint = new Vector3(position.x - width, position.y, position.z);
        endPoint = new Vector3(position.x + width, position.y, position.z);
    }

    void Update()
    {
        mask.fillAmount = current / max;
    }
}
