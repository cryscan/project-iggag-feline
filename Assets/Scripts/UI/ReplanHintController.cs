using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplanHintController : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance.startState == GameState.Play)
            gameObject.SetActive(false);
    }
}