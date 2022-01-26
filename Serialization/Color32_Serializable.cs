using System;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public struct Color32_Serializable
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public Color32 color32
        {
            get { return new Color32(r, g, b, a); }
            set { r = value.r; g = value.g; b = value.b; a = value.a; }
        }
        public Color color
        {
            get { return new Color(r, g, b, a); }
            set { Color32 c = value; r = c.r; g = c.g; b = c.b; a = c.a; }
        }

        public static implicit operator Color32(Color32_Serializable instance)
        {
            return instance.color32;
        }

        public static implicit operator Color(Color32_Serializable instance)
        {
            return instance.color32;
        }

        public static implicit operator Color32_Serializable(Color32 color)
        {
            return new Color32_Serializable { color32 = color };
        }

        public static implicit operator Color32_Serializable(Color color)
        {
            return new Color32_Serializable { color32 = color };
        }

        //public Color32_Serializable(byte r, byte g, byte b, byte a)
        //{
        //    this.r = r;
        //    this.g = g;
        //    this.b = b;
        //    this.a = a;
        //}

        //public Color32_Serializable(Color32 sourceColor)
        //{
        //    r = sourceColor.r;
        //    g = sourceColor.g;
        //    b = sourceColor.b;
        //    a = sourceColor.a;
        //}

        //public Color32 ToColor32()
        //{
        //    return new Color32(r, g, b, a);
        //}
    }
}
