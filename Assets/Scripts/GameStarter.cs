using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] bool play = false;

    void Start()
    {
        if (GameManager.instance.currentState == GameState.Start)
        {
            if (!play) GameManager.instance.StartPlan();
            else GameManager.instance.StartPlay();
        }
    }
}
