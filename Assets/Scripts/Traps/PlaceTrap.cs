using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrap : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float radius;
    [SerializeField] LayerMask layers;
    [SerializeField] LayerMask blockLayers;
    [SerializeField] float count;
    [SerializeField] Sprite sprite;
    [SerializeField] string trapType;

    [SerializeField] GameObject ball;
    Camera _camera;
    CounterController Counter;

    private void Awake()
    {
        _camera = Camera.main;
        ball.transform.localScale = new Vector3(radius, radius, radius);
        Cursor.visible = false;
        Counter = GameObject.FindGameObjectWithTag("Counter").GetComponent<CounterController>();
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameState.Plan)
        {
            var valid = CheckPosition();           
            if (valid && count > 0)
            {
                Debug.Log(prefab);
                ball.SetActive(true);
                Cursor.SetCursor(sprite.texture, new Vector2(16, 16), CursorMode.ForceSoftware);
            }
            else
            {
               ball.SetActive(false);
            //    Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
            //    Cursor.visible = true;
            }

            if (trapType == "frozen")
                Counter.SetFrozenCount(count);
            else
                Counter.SetDistractionCount(count);

            if (valid && Input.GetMouseButtonDown(0))
                InstantiateTrap();
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            ball.SetActive(false);
        }
        

    }

    bool CheckPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, blockLayers))
        {
            return hit.collider.gameObject.CompareLayers(layers);
        }
        else
            return false;
    }

    void InstantiateTrap()
    {
        if (count == 0)
        {
            Debug.Log("Run out of traps!");
            return;
        }

        var extents = prefab.GetComponent<Collider>().bounds.extents;

        

        Vector3 position = new Vector3(Mathf.Round(transform.position.x),
                                             0f,
                                             Mathf.Round(transform.position.z));


        GameObject trap = GameObject.Instantiate(prefab, position, Quaternion.identity);

        count--;

        ScheduleManager.instance.AddSchedule(prefab, position);
        trap.GetComponent<TrapBase>()?.Activate();
    }
}