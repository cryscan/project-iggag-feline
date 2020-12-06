using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue", fileName = "New Dialogue")]
public class Dialogue : ScriptableObject
{
    public TextAsset text;
    public bool pause = false;
    public bool once = true;

    public AudioClip audio;
    public bool voice = false;

    public bool triggered { get; set; } = false;
    public string[] sentences { get; private set; }

    void OnEnable()
    {
        triggered = false;
        if (text) sentences = text.text.Split('\n');
    }
}
