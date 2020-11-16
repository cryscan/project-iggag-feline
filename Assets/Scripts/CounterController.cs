using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] CounterObject counter;

    void Update()
    {
        text.text = counter.count.ToString("00");
    }
}
