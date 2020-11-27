using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Counter Object", fileName = "New Counter Object")]
public class CounterObject : ScriptableObject
{
    public Sprite sprite;
    public bool selected;
    public int count;
}
