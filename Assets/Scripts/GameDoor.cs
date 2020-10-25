using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventBus.Publish<GoalEvent>(new GoalEvent());
        } 
    }
}

public class GoalEvent
{
    
}
