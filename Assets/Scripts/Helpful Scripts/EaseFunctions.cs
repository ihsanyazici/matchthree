using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EaseFunctions
{
    public static float EaseIn(float t)
    {
        return t * t;
    }

    public static float EaseOut(float t)
    {
        return Flip(Square(Flip(t)));
    }

    public static float EaseInOut(float t)
    {
        return Mathf.Lerp(EaseIn(t), EaseOut(t), t);
    }

    //  Helpful Functions
    static float Square(float x)
    {
        return x * x;
    }
    static float Flip(float x)
    {
        return 1 - x;
    }
}
