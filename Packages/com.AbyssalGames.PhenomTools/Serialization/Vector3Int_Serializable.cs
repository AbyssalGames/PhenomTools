using System;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public struct Vector3Int_Serializable
    {
        public static readonly Vector3Int_Serializable one = new Vector3Int_Serializable(1, 1, 1);
        public static readonly Vector3Int_Serializable zero = new Vector3Int_Serializable(0, 0, 0);

        public int x;
        public int y;
        public int z;

        public Vector3Int_Serializable(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3Int_Serializable(Vector3Int vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public Vector3Int ToUnserializable()
        {
            return new Vector3Int(x, y, z);
        }
    }
}