using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue", fileName = "New Dialogue")]
public class Dialogue : ScriptableObject
{
    [Header("Dialogue")]
    public TextAsset text;
    public bool pause = false;
    public bool once = true;

    [Header("Audio")]
    public AudioClip audio;
    [Range(0, 1)]
    public float volume = 1;
    [Range(-3, 3)]
    public float pitch = 1;
    public bool voice = false;

    public bool triggered { get; set; } = false;
    public string[] sentences { get; private set; }

    void OnEnable()
    {
        triggered = false;
        if (text) sentences = text.text.Split('\n');
    }
}
