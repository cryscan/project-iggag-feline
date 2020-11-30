using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangeIndicator))]
public class PlaceTrap : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Sprite sprite;
    [SerializeField] int button = 0;

    [Header("Layers")]
    [SerializeField] LayerMask layers;
    [SerializeField] LayerMask blockLayers;

    [Header("Count")]
    [SerializeField] int initialCount;
    [SerializeField] CounterObject counter;

    Camera _camera;
    TrapBase trap;
    RangeIndicator indicator;

    private void Awake()
    {
        _camera = Camera.main;
        trap = prefab.GetComponent<TrapBase>();
        indicator = GetComponent<RangeIndicator>();
        counter.count = initialCount;
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameState.Plan)
        {
            Vector3? center;
            var valid = CheckPosition(out center);
            if (valid && counter.count > 0)
            {
                if (sprite) Cursor.SetCursor(sprite.texture, new Vector2(16, 16), CursorMode.ForceSoftware);

                indicator.center = center.Value;
                indicator.radius = trap.GetRange();

                counter.selected = true;
            }
            else
            {
                indicator.radius = 0;
                counter.selected = false;
            }

            var clicked = Input.GetMouseButtonDown(button);
            if (valid && clicked && !DialogueManager.instance.running)
                InstantiateTrap();
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            indicator.radius = 0;
        }
    }

    bool CheckPosition(out Vector3? position)
    {
        position = null;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, blockLayers))
        {
            position = hit.point;
            return hit.collider.gameObject.CompareLayers(layers);
        }
        else
            return false;
    }

    void InstantiateTrap()
    {
        if (counter.count == 0)
        {
            Debug.Log("Run out of traps!");
            return;
        }

        var extents = prefab.GetComponent<Collider>().bounds.extents;



        Vector3 position = new Vector3(Mathf.Round(transform.position.x),
                                             Mathf.Round(transform.position.y),
                                             Mathf.Round(transform.position.z));


        GameObject trap = GameObject.Instantiate(prefab, position, Quaternion.identity);

        counter.count--;

        ScheduleManager.instance.AddSchedule(prefab, position);
        trap.GetComponent<TrapBase>()?.Activate();
    }
}