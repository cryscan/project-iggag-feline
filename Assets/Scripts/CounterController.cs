using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    [SerializeField] CounterObject counter;

    Image image;
    Text text;

    void Awake()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();

        image.sprite = counter.sprite;
    }

    void Update()
    {
        text.text = counter.count.ToString("00");
    }
}
