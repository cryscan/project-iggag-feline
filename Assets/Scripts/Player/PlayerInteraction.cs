using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InteractionController))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] LayerMask promptLayers;
    [SerializeField] float promptDistance = 2;
    Interactable prompting;

    [Header("Collect")]
    [SerializeField] LayerMask collectLayers;
    [SerializeField] float collectDistance = 2;

    [Header("Hide")]
    [SerializeField] LayerMask hideLayers;
    [SerializeField] float hideDistance = 2;


    Camera _camera;
    InteractionController controller;

    void Awake()
    {
        _camera = Camera.main;
        controller = GetComponent<InteractionController>();
    }

    void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, promptDistance, promptLayers))
        {
            var _object = hit.collider.gameObject;
            SetInteracting(_object);
        }
        else SetDisinteracting();

        if (Input.GetButtonDown("Collect"))
        {
            if (Physics.Raycast(ray, out hit, collectDistance, collectLayers))
            {
                var interactable = hit.collider.gameObject.GetComponent<Interactable>();
                // Broadcase both drop and collect events. The interactors will determine which to react.
                EventBus.Publish(new InteractEvent(gameObject, interactable, InteractionType.Drop));
                EventBus.Publish(new InteractEvent(gameObject, interactable, InteractionType.Collect));
            }
            else EventBus.Publish(new InteractEvent(gameObject, null, InteractionType.Drop));
        }

        if (Input.GetButtonDown("Hide"))
        {
            if (Physics.Raycast(ray, out hit, hideDistance, hideLayers))
            {
                var interactable = hit.collider.gameObject.GetComponent<Interactable>();
                EventBus.Publish(new InteractEvent(gameObject, interactable, InteractionType.ComeOut));
                EventBus.Publish(new InteractEvent(gameObject, interactable, InteractionType.Hide));
            }
            else EventBus.Publish(new InteractEvent(gameObject, null, InteractionType.ComeOut));
        }
    }

    void SetInteracting(GameObject _object)
    {
        if (prompting && prompting.gameObject == _object) return;
        //Debug.Log($"Player interacting {_object.name}");

        var next = _object.GetComponent<Interactable>();
        prompting = next;
        EventBus.Publish(new PlayerPromptEvent(prompting));
    }

    void SetDisinteracting()
    {
        if (prompting != null) EventBus.Publish(new PlayerUnfocusEvent(prompting));
        prompting = null;
    }
}

public class PlayerPromptEvent
{
    public Interactable _object;

    public PlayerPromptEvent(Interactable _object)
    {
        this._object = _object;
    }
}

public class PlayerUnfocusEvent
{
    public Interactable _object;

    public PlayerUnfocusEvent(Interactable _object)
    {
        this._object = _object;
    }
}