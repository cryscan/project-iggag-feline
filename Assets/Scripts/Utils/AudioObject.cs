using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Add more if necessary
public enum AudioType
{
    Fire,
    Missile,
    Run,
    Jump,
    Orb,
    PlayerHurt,
    EnemyHurt,
    Ripper,
}

[CreateAssetMenu(menuName = "Audio Object", fileName = "New Audio Object")]
public class AudioObject : ScriptableObject
{
    public AudioClip clip;
    public AudioType type;

    public float volume = 1;
    public float pitch = 1;
}
