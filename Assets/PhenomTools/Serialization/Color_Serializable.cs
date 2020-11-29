using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;

namespace PhenomTools
{
    [Serializable]
    public struct Color_Serializable
    {
        public float r, g, b, a;

        public Color Color
        {
            get { return new Color(r, g, b, a); }
            set { r = value.r; g = value.g; b = value.b; a = value.a; }
        }

        //makes this class usable as Color, Color normalColor = mySerializableColor;
        public static implicit operator Color(Color_Serializable instance)
        {
            return instance.Color;
        }

        //makes this class assignable by Color, SerializableColor myColor = Color.white;
        public static implicit operator Color_Serializable(Color color)
        {
            return new Color_Serializable { Color = color };
        }
    }
}
