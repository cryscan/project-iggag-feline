using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static float Fallout(this float origin, float target, float fallout)
    {
        return Mathf.Lerp(origin, target, 1 - Mathf.Exp(-fallout * Time.deltaTime));
    }

    public static float FalloutUnscaled(this float origin, float target, float fallout)
    {
        return Mathf.Lerp(origin, target, 1 - Mathf.Exp(-fallout * Time.unscaledDeltaTime));
    }
}
