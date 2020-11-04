/* Stores the display name and control sprite of an input action.
 * @Zhenyuan Zhang
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Control Prompt", menuName = "Control Prompt/Control Prompt")]
public class ControlPromptObject : ScriptableObject
{
    public InteractionType action;
    public string displayName;
    public Sprite sprite;
}
