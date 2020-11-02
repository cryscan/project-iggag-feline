using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractionController))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] LayerMask promptLayers;
    [SerializeField] float promptDistance = 2;
    Interactable prompting;

    Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
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
    }

    void SetInteracting(GameObject _object)
    {
        if (prompting && prompting.gameObject == _object) return;
        Debug.Log($"Player interacting {_object.name}");

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