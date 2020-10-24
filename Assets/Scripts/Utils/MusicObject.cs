using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    Brinstar,
    ItemRoom,
    Boss,
    Collectable,
    Warning,
    Kraid,
    Title,
    Tourian,
    Ending,
}

[CreateAssetMenu(menuName = "Music Object", fileName = "New Music Object")]
public class MusicObject : ScriptableObject
{
    public AudioClip clip;
    public MusicType type;

    public float volume = 1;
    public float pitch = 1;
    public bool loop = true;
}
