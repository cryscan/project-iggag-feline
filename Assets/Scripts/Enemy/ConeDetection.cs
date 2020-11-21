using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DetectionType
{
    Camera,
    Guard,
}

public class ConeDetection : MonoBehaviour
{
    [SerializeField] Transform eye;
    [SerializeField] string targetTag;

    [Header("Detection")]
    [SerializeField] float range = 10;
    [SerializeField] float fov = 60;
    [SerializeField] LayerMask layers;
    [SerializeField] DetectionType type;

    public bool detected { get; private set; } = false;

    [Header("Follow")]
    [SerializeField] float angleLimit = 60;
    [SerializeField] float fallout = 10;

    GameObject[] targets;
    GameObject target;

    void Awake()
    {
        targets = GameObject.FindGameObjectsWithTag(targetTag);
    }

    void Update()
    {
        foreach (var target in targets) Detect(target);
        Follow();
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = eye.localToWorldMatrix;
        Gizmos.DrawFrustum(Vector3.zero, fov, range, 0.1f, 1);
    }

    void Detect(GameObject target)
    {
        var position = target.transform.position;
        var direction = position - eye.position;
        direction.y = 0;

        var distance = direction.magnitude;
        var angle = Vector3.Angle(eye.forward, direction);

        bool lost = true;
        if (angle < fov / 2 && distance <= range)
        {
            // Debug.Log($"[Detect] {distance}, {angle}");

            Ray ray = new Ray(eye.position, direction);
            RaycastHit hit;

            if (Physics.SphereCast(ray, 0.1f, out hit, distance, layers) && hit.collider.gameObject == target)
            {
                if (!detected)
                {
                    Debug.Log($"[Detect] {hit.collider.gameObject}");
                    // Debug.Break();

                    this.target = target;
                    EventBus.Publish(new DetectEvent(gameObject, target, type, fov));
                    detected = true;
                }
                lost = false;
            }
        }

        if (lost && detected)
        {
            EventBus.Publish(new LossTargetEvent(gameObject, target, type, fov, position));
            detected = false;
        }
    }

    void Follow()
    {
        Vector3 direction;
        if (detected)
        {
            direction = target.transform.position - eye.position;
            direction.y = 0;
        }
        else direction = transform.forward;

        var angle = Vector3.SignedAngle(eye.forward, direction, eye.up);
        angle = Mathf.Clamp(angle, -angleLimit, angleLimit);

        float rotation = 0.0f.Fallout(angle, fallout);
        eye.Rotate(0, rotation, 0, Space.Self);
    }
}

public class DetectEvent
{
    public GameObject subject, target;
    public DetectionType type;
    public float fov;

    public DetectEvent(GameObject subject, GameObject target, DetectionType type, float fov)
    {
        this.subject = subject;
        this.target = target;
        this.type = type;
        this.fov = fov;
    }
}

public class LossTargetEvent
{
    public GameObject subject, target;
    public DetectionType type;
    public float fov;
    public Vector3 spotPoint;

    public LossTargetEvent(GameObject subject, GameObject target, DetectionType type, float fov, Vector3 spotPoint)
    {
        this.subject = subject;
        this.target = target;
        this.type = type;
        this.spotPoint = spotPoint;
        this.fov = fov;
    }
}