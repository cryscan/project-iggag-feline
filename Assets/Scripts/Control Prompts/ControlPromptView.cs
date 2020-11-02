using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPromptView : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Image image;

    void Awake()
    {
        image.preserveAspect = true;
    }

    public void SetControlPrompt(ControlPromptObject controlPrompt)
    {
        text.text = controlPrompt.displayName;
        image.sprite = controlPrompt.sprite;
    }
}
