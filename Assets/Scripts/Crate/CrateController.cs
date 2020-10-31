using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideEvent
{
	public Vector3 position;
    public PlayerHideEvent(Vector3 position)
    {
        this.position = position;
    }
}

public class PlayerUnhideEvent
{
    public PlayerUnhideEvent(){}
}

public class PlayerHideExposeEvent
{
    public PlayerHideExposeEvent(){}
}

public class CrateController : MonoBehaviour
{
	private bool inside = false;
	private bool hidden = false;
    private void OnTriggerEnter(Collider other)
    {
    	if (other.gameObject.CompareTag("Player") && hidden == false && inside == false) {
    		inside = true;
    		hidden = true;
    		EventBus.Publish(new PlayerHideEvent(transform.position));
    	}
    }

    void LateUpdate()
    {
    	if(hidden == true && Input.GetKeyDown(KeyCode.Space))
    	{
    		Debug.Log("CAN EXIT");
    		hidden = false;
    		EventBus.Publish(new PlayerUnhideEvent());
    	}
    }

    private void OnTriggerExit(Collider other)
    {
    	if (other.gameObject.CompareTag("Player"))
    	{
    		inside = false;
    	}
    }
}
