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

        public Color32_Serializable(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color32_Serializable(Color32 sourceColor)
        {
            r = sourceColor.r;
            g = sourceColor.g;
            b = sourceColor.b;
            a = sourceColor.a;
        }

        public Color32 ToColor32()
        {
            return new Color32(r, g, b, a);
        }
    }
}
