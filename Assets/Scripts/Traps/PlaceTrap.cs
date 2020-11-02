using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTrap : MonoBehaviour
{
    [System.Serializable]
    public class Config
    {
        public GameObject prefab;
        public int count;
        public string description;
    }

    [SerializeField] Transform look;
    // [SerializeField] GameObject prefab;
    [SerializeField] List<Config> config;

    [SerializeField] LayerMask forbiddenLayer;

    int index = 0;
    public GameObject prefab { get => config[index].prefab; }
    public int count { get => config[index].count; }
    public string description { get => config[index].description; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && config.Count > 1) index = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && config.Count > 2) index = 2;

        if (GameManager.instance.currentState == GameState.Plan && Input.GetMouseButtonDown(0))
            InstantiateTrap();
    }

    void InstantiateTrap()
    {
        if (count == 0)
        {
            Debug.Log("Run out of traps!");
            return;
        }

        var extents = prefab.GetComponent<Collider>().bounds.extents;

        Vector3 position = new Vector3(Mathf.Round(look.position.x),
                                             0f,
                                             Mathf.Round(look.position.z));

        Collider[] hitColliders = Physics.OverlapBox(position, extents, Quaternion.identity, forbiddenLayer);
        if (hitColliders.Length > 0)
        {
            Debug.Log("Cannot place trap here!");
            return;
        }

        GameObject trap = GameObject.Instantiate(prefab, position, Quaternion.identity);
        config[index].count--;

        ScheduleManager.instance.AddSchedule(prefab, position);
        trap.GetComponent<TrapHandler>()?.Activate();
    }
}