using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, Interactor
{
    [System.Serializable]
    struct CollectableTarget
    {
        public CollectableType type;
        public Transform target;
    }

    [SerializeField] Transform head;

    [Tooltip("Pickup center of all types of collectables")]
    [SerializeField] CollectableTarget[] _targets;
    Dictionary<CollectableType, Transform> targets = new Dictionary<CollectableType, Transform>();

    [SerializeField] float dropRange = 2;

    [Header("Fallout")]
    [SerializeField] float positionFallout = 20;
    [SerializeField] float rotationFallout = 10;

    DropPoint[] dropPoints;

    public Collectable holding { get; private set; }

    Transform target;

    void Awake()
    {
        foreach (var x in _targets) targets.Add(x.type, x.target);
        dropPoints = FindObjectsOfType<DropPoint>();
    }

    void LateUpdate()
    {
        if (holding)
        {
            Debug.Assert(target != null);

            var position = holding.transform.position;
            var rotation = holding.transform.rotation;

            position = position.Fallout(target.position, positionFallout);
            rotation = rotation.Fallout(target.rotation, rotationFallout);

            holding.transform.SetPositionAndRotation(position, rotation);
        }
    }

    public InteractionType[] GetInteractions(Interactable interactable)
    {
        List<InteractionType> interactions = new List<InteractionType>();
        var collectabe = interactable.GetComponent<Collectable>();
        if (collectabe)
        {
            if (holding == collectabe) interactions.Add(InteractionType.Drop);
            else if (collectabe.subject == null) interactions.Add(InteractionType.Collect);
        }
        return interactions.ToArray();
    }

    public void Collect(Collectable collectable)
    {
        if (holding) Drop();
        holding = collectable;
        target = targets[holding.type];
        EventBus.Publish(new CollectEvent(this, holding));
    }

    public void Drop()
    {
        if (holding == null)
        {
            Debug.LogError($"[Collectable] has no subject yet");
            return;
        }

        if (holding.type == CollectableType.Crate) NormalDrop();
        else
        {
            var query = from point in dropPoints
                        let distance = Vector3.Distance(transform.position, point.transform.position)
                        where distance < dropRange
                        orderby point.priority, distance
                        select point;
            var candidates = query.ToArray();

            if (candidates.Length == 0) NormalDrop();
            else
            {
                target = candidates[0].transform;
                StartCoroutine(DropCoroutine());
            }
        }
    }

    void NormalDrop()
    {
        EventBus.Publish(new DropEvent(this, holding, transform.position));
        holding = null;
    }

    IEnumerator DropCoroutine()
    {
        var timeLimit = 5 / positionFallout;
        float timer = 0;

        while (timer < timeLimit && Vector3.Distance(holding.transform.position, target.position) < 0.1)
            yield return null;

        EventBus.Publish(new DropEvent(this, holding, target.position));
        holding = null;
    }
}

class CollectEvent
{
    public Inventory subject;
    public Collectable _object;

    public CollectEvent(Inventory subject, Collectable _object)
    {
        this.subject = subject;
        this._object = _object;
    }
}

class DropEvent
{
    public Inventory subject;
    public Collectable _object;
    public Vector3 location;

    public DropEvent(Inventory subject, Collectable _object, Vector3 position)
    {
        this.subject = subject;
        this._object = _object;
        this.location = position;
    }
}