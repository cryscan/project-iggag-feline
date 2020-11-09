using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance.currentState == GameState.Start)
            GameManager.instance.StartPlan();
    }
}
