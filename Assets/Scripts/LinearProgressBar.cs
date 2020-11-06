using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearProgressBar : MonoBehaviour
{
    [SerializeField] Image mask;
    public float max;
    public float current;

    void Update()
    {
        mask.fillAmount = current / max;
    }
}
