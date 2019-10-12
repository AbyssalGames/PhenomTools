using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Color32_Serializable
{
    public byte[] rgba;

    public Color32_Serializable(Color32 sourceColor)
    {
        rgba = new byte[4];

        rgba[0] = sourceColor.r;
        rgba[1] = sourceColor.g;
        rgba[2] = sourceColor.b;
        rgba[3] = sourceColor.a;
    }

    public Color32 ToColor32()
    {
        return new Color32(rgba[0], rgba[1], rgba[2], rgba[3]);
    }
}
