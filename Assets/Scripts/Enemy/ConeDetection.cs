using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetection : MonoBehaviour
{
    private GameObject player;
    public GameObject center;
    public GameObject dLight;
    public float detectDistance = 10.0f;
    public float detectAngle = 60.0f;

    private void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Error: Unable to locate player gameobject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position + player.GetComponent<CharacterController>().center - transform.position;
            if (Vector3.Angle(center.transform.position - transform.position, direction) < detectAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, detectDistance))
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject == player)
                    {
                        //Debug.Log("Player detected");
                        dLight.GetComponent<Light>().color = Color.red;
                        EventBus.Publish<DetectEvent>(new DetectEvent(true));
                    }

                }
            }
        }
    }
}

public class DetectEvent 
{
    public bool detected;
    public DetectEvent(bool d) { detected = d; }
}