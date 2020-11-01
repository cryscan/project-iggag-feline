using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    struct CollectableTarget
    {
        public CollectableType type;
        public Transform target;
    }

    [SerializeField] CollectableTarget[] _targets;
    Dictionary<CollectableType, Transform> targets = new Dictionary<CollectableType, Transform>();

    [SerializeField] float range;

    [Header("Fallout")]
    [SerializeField] float positionFallout = 20;
    [SerializeField] float rotationFallout = 10;

    public Collectable holding { get; private set; }
    Transform target;

    void Awake()
    {
        foreach (var x in _targets) targets.Add(x.type, x.target);
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

    public void Collect(Collectable collectable)
    {
        if (holding) Drop();

        holding = collectable;
        collectable.Collect(this);

        target = targets[holding.type];

        EventBus.Publish(new InventoryCollectEvent(this, holding));
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
            var dropPoints = GameContext.instance.dropPoints;
            var query = from point in dropPoints
                        let distance = Vector3.Distance(transform.position, point.transform.position)
                        where distance < range
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
        holding.Drop();
        EventBus.Publish(new InventoryDropEvent(this, holding, transform.position));
        holding = null;
    }

    IEnumerator DropCoroutine()
    {
        var timeLimit = 5 / positionFallout;
        float timer = 0;

        while (timer < timeLimit && Vector3.Distance(holding.transform.position, target.position) < 0.1)
            yield return null;

        holding.Drop();
        EventBus.Publish(new InventoryDropEvent(this, holding, target.position));
        holding = null;
    }
}

class InventoryCollectEvent
{
    public Inventory subject;
    public Collectable _object;

    public InventoryCollectEvent(Inventory subject, Collectable _object)
    {
        this.subject = subject;
        this._object = _object;
    }
}

class InventoryDropEvent
{
    public Inventory subject;
    public Collectable _object;
    public Vector3 location;

    public InventoryDropEvent(Inventory subject, Collectable _object, Vector3 position)
    {
        this.subject = subject;
        this._object = _object;
        this.location = position;
    }
}