using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPromptLister : MonoBehaviour
{
    public ControlPromptDatabaseObject database;
    [SerializeField] GameObject prefab;
    [SerializeField] bool generateOnAwake = true;

    public Dictionary<InteractionType, GameObject> children { get; private set; } = new Dictionary<InteractionType, GameObject>();

    void Awake()
    {
        if (generateOnAwake) Generate();
    }

    /// <summery>
    /// Instantiate all entries from the database.
    /// </summery>
    public void Generate()
    {
        if (database == null) return;
        foreach (var controlPrompt in database)
        {
            GameObject obj = Instantiate(prefab, transform);
            var view = obj.GetComponent<ControlPromptView>();
            view.SetControlPrompt(controlPrompt);

            children.Add(controlPrompt.action, obj);
        }
    }

    public GameObject Find(InteractionType action)
    {
        if (children.ContainsKey(action)) return children[action];
        else return null;
    }

    public void SetActiveAll(bool active)
    {
        foreach (var obj in children.Values)
            obj.SetActive(active);
    }
}
