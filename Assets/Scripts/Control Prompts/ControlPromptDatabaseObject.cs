/* Stores a list of control prompts.
 * @Zhenyuan Zhang
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Control Prompt Database", menuName = "Control Prompt/Database")]
public class ControlPromptDatabaseObject : ScriptableObject, IEnumerable<ControlPromptObject>
{
    [SerializeField] List<ControlPromptObject> controlPrompts;
    public int size { get => controlPrompts.Count; }

    public ControlPromptObject Find(InteractionType action)
    {
        return controlPrompts.Find(control => control.action == action);
    }

    public int FindIndex(InteractionType action)
    {
        return controlPrompts.FindIndex(control => control.action == action);
    }

    public IEnumerator<ControlPromptObject> GetEnumerator()
    {
        return controlPrompts.GetEnumerator() as IEnumerator<ControlPromptObject>;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
