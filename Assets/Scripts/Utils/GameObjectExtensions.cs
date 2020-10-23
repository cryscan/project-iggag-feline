using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool CompareLayers(this GameObject _object, LayerMask layers)
    {
        return ((1 << _object.layer) & layers) != 0;
    }
}
