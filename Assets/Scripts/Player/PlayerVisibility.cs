using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerLight))]
public class PlayerVisibility : MonoBehaviour
{
    [SerializeField] float basicVisibility = 0.5f;
    [SerializeField] float fallout = 10;

    [SerializeField] VirtualLight[] lights;
    [SerializeField] LayerMask blockLayers;

    [SerializeField] Transform head;

    public float visibility { get; private set; }
    PlayerLight playerLight;

    void Awake()
    {
        playerLight = GetComponent<PlayerLight>();
        lights = FindObjectsOfType<VirtualLight>();
    }

    void Update()
    {
        float visibility = basicVisibility;
        foreach (var _light in lights)
        {
            var direction = _light.transform.position - head.position;
            var distance = direction.magnitude;

            Ray ray = new Ray(head.position, direction);
            if (!Physics.Raycast(ray, distance, blockLayers)) visibility += _light.IntensityAtPoint(head.position);
        }

        visibility *= playerLight.intensity;

        this.visibility = this.visibility.Fallout(visibility, fallout);
    }
}
