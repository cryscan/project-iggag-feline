using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrap : MonoBehaviour
{
    [SerializeField] Transform look;
    [SerializeField] GameObject prefab;

    [SerializeField] LayerMask forbiddenLayer;

    Vector3 halfExtents;

    private void Awake()
    {
        halfExtents = prefab.GetComponent<Collider>().bounds.extents;
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameState.Plan && Input.GetMouseButtonDown(0))
            InstantiateTrap();
    }

    void InstantiateTrap()
    {
        int counter = GameManager.instance.trapCounter;
        if (counter == 0)
        {
            Debug.Log("Run out of traps!");
            return;
        }

        Vector3 targetPosition = new Vector3(Mathf.Round(look.position.x),
                                             0f,
                                             Mathf.Round(look.position.z));

        Collider[] hitColliders = Physics.OverlapBox(targetPosition, halfExtents,
                                                     Quaternion.identity, forbiddenLayer);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Cannot place trap here!");
            return;
        }

        GameObject trap = GameObject.Instantiate(prefab, targetPosition, Quaternion.identity);
        GameManager.instance.ConsumeTrap();

        ScheduleManager.instance.AddSchedule(prefab, targetPosition);
    }
}