using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectionType
{
    Camera,
    Guard,
}

public class ConeDetection : MonoBehaviour
{
    [SerializeField] Transform eye;

    [SerializeField] float detectDistance = 10;
    [SerializeField] float detectAngle = 60;
    [SerializeField] LayerMask detectLayers;
    [SerializeField] DetectionType detectionType;

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
            if (Physics.Raycast(ray, out hit, detectDistance, detectLayers) && hit.collider.gameObject == player)
                EventBus.Publish(new DetectEvent(gameObject, detectionType, player.transform.position));
        }
    }
}

public class DetectEvent
{
    public GameObject subject;
    public DetectionType type;
    public Vector3 spotPoint;

    public DetectEvent(GameObject subject, DetectionType type, Vector3 spotPoint)
    {
        this.subject = subject;
        this.type = type;
        this.spotPoint = spotPoint;
    }
}