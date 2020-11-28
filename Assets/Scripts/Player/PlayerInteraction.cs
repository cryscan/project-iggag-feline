using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] LayerMask rayCastLayers;
    [SerializeField] LayerMask interactLayers;
    [SerializeField] float distance = 2;

    public GameObject interacting { get; private set; } = null;

    Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        DetectInteraction();
    }

    void DetectInteraction()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, rayCastLayers))
        {
            var _object = hit.collider.gameObject;
            if (!_object.CompareLayers(interactLayers)) return;

            if (interacting != _object)
            {
                if (interacting) EventBus.Publish(new PlayerDisinteractEvent() { target = interacting });
                interacting = _object;
                EventBus.Publish(new PlayerInteractEvent() { target = interacting });
            }
        }
        else if (interacting)
        {
            EventBus.Publish(new PlayerDisinteractEvent() { target = interacting });
            interacting = null;
        }
    }
}

public enum InteractionType
{
    PickUp,
    Drop,
    Hide,
    ComeOut,
    EnterSimulation,
    StartHeist,
    OpenSettings,
    ExitGame,
}

public class PlayerInteractEvent : IEvent { public GameObject target; }
public class PlayerDisinteractEvent : IEvent { public GameObject target; }

public class PlayerPromptEvent : IEvent
{
    public GameObject target;
    public InteractionType type;

    public PlayerPromptEvent(GameObject target, InteractionType type)
    {
        this.target = target;
        this.type = type;
    }
}

public class PlayerDispromptEvent : IEvent { public InteractionType type; }