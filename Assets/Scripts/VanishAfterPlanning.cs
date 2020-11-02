using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class VanishAfterPlanning : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.instance.currentState == GameState.Play)
        {
            GetComponent<CinemachineVirtualCamera>().Priority = 0; 
        }
    }
}
