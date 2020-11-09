using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    [SerializeField] Text frozenCount;
    [SerializeField] Text distractionCount;

    public void SetFrozenCount(float num)
    {
        frozenCount.text = num.ToString();
    }

    public void SetDistractionCount(float num)
    {
        distractionCount.text = num.ToString();
    }
}
