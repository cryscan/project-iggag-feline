using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using ReGoap.Core;

public class GuardStateView : MonoBehaviour
{
    [System.Serializable]
    struct ActionText
    {
        public string actionName;
        public string text;
    }

    [SerializeField] Text text;
    [SerializeField] List<ActionText> _actionTexts;
    Dictionary<string, string> actionTexts = new Dictionary<string, string>();

    IReGoapAgent<string, object> agent;

    void Awake()
    {
        agent = GetComponent<IReGoapAgent<string, object>>();
        foreach (var actionText in _actionTexts) actionTexts.Add(actionText.actionName, actionText.text);
    }

    void Update()
    {
        var actions = agent?.GetActionsSet();
        var currentAction = actions?.Find(x => x.IsActive());
        var name = currentAction?.GetName();

        if (name != null && actionTexts.ContainsKey(name))
            text.text = actionTexts[name];
        else text.text = "";
    }
}
