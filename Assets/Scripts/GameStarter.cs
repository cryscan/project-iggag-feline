using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    void Start()
    {
    	Debug.Log(GameManager.instance.currentState);
        if (GameManager.instance.currentState == GameState.Start)
        {
            GameManager.instance.StartPlan();
        }
    }
}
