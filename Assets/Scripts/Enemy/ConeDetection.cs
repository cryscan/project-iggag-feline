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

    public bool detected { get; private set; } = false;

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

        bool lost = false;

        if (angle < detectAngle && distance <= detectDistance)
        {
            Ray ray = new Ray(eye.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, detectDistance, detectLayers) && hit.collider.gameObject == player)
            {
                if (!detected) EventBus.Publish(new DetectEvent(gameObject, detectionType, detectAngle));
                detected = true;
            }
            else lost = true;
        }
        else lost = true;

        if (lost && detected)
        {
            EventBus.Publish(new LossTargetEvent(gameObject, detectionType, player.transform.position, detectAngle));
            detected = false;
        }
    }
}

public class DetectEvent
{
    public GameObject subject;
    public DetectionType type;
    public float detectAngle;

    public DetectEvent(GameObject subject, DetectionType type, float detectAngle)
    {
        this.subject = subject;
        this.type = type;
        this.detectAngle = detectAngle;
    }
}

public class LossTargetEvent
{
    public GameObject subject;
    public DetectionType type;
    public Vector3 spotPoint;
    public float detectAngle;

    public LossTargetEvent(GameObject subject, DetectionType type, Vector3 spotPoint, float detectAngle)
    {
        this.subject = subject;
        this.type = type;
        this.spotPoint = spotPoint;
        this.detectAngle = detectAngle;
    }
}