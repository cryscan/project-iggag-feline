using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerLight))]
public class PlayerVisibility : MonoBehaviour
{
    [SerializeField] float _visibility;
    public float visibility { get => _visibility; }
    [SerializeField] float fallout = 10;

    [SerializeField] List<VirtualLight> lights;
    [SerializeField] LayerMask blockLayers;

    [SerializeField] Transform head;

    PlayerLight playerLight;

    void Awake()
    {
        playerLight = GetComponent<PlayerLight>();
    }

    void Update()
    {
        float visibility = 0;
        foreach (var _light in lights)
        {
            var direction = _light.transform.position - head.position;
            var distance = direction.magnitude;

            Ray ray = new Ray(head.position, direction);
            if (!Physics.Raycast(ray, distance, blockLayers)) visibility += _light.IntensityAtPoint(head.position);
        }

        visibility *= playerLight.intensity;

        _visibility = _visibility.Fallout(visibility, fallout);
    }
}
