using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] GameObject planningControls;

    void Update()
    {
        planningControls.SetActive(GameManager.instance.currentState == GameState.Plan);
    }
    
}
