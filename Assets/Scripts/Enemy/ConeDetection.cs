using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeDetection : MonoBehaviour
{
    [SerializeField] Transform eye;
    [SerializeField] Light _light;

    [SerializeField] float detectDistance = 10;
    [SerializeField] float detectAngle = 60;
    [SerializeField] LayerMask detectLayers;

    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player) Debug.LogError("Error: Unable to locate player gameobject.");
    }

    void Update()
    {
        if (!player) return;

        var direction = player.transform.position - eye.position;
        direction.y = 0;

        var distance = direction.magnitude;
        var angle = Vector3.Angle(eye.forward, direction);

        if (angle < detectAngle && distance <= detectDistance)
        {
            Ray ray = new Ray(eye.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, detectDistance, detectLayers))
            {
                if (hit.collider.gameObject == player)
                {
                    _light.color = Color.red;
                    EventBus.Publish<DetectEvent>(new DetectEvent(gameObject));
                }
            }
        }
    }
}

public class DetectEvent
{
    public GameObject subject;
    public DetectEvent(GameObject subject) => this.subject = subject;
}