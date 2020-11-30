using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeistBoard : MonoBehaviour
{
    [SerializeField] bool _practice = false;
    public bool practice { get => _practice; }

    [SerializeField] bool plan = true;

    [SerializeField] string sceneName;

    public void StartHeist()
    {
        if (!GameManager.instance.transiting)
        {
            if (plan) GameManager.instance.EnterPlanScene(sceneName, true);
            else GameManager.instance.EnterPlayScene(sceneName, true);
        }
    }
}
