using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{

    public static Color PingPong(this Color color, Color a, Color b)
    {
        return Color.Lerp(a, b, Mathf.PingPong(Time.time, 1));
    }
}
