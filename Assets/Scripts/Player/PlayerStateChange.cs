using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateChange : MonoBehaviour
{
    PlayerLook playerLook;
    PlayerMovement playerMovement;
    CharacterController controller;

    private void Awake()
    {
        playerLook = GetComponent<PlayerLook>();
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        var state = GameManager.instance.currentState;
        if (state == GameState.Plan)
        {
            playerLook.enabled = false;
            playerMovement.enabled = false;
            // controller.enabled = false;
        }
        else if (state == GameState.Play)
        {
            playerLook.enabled = true;
            playerMovement.enabled = true;
            // controller.enabled = true;
        }
    }
}
