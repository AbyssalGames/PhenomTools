using System;
using UnityEngine;

namespace PhenomTools
{
    [Serializable]
    public struct Vector3_Serializable
    {
        public float x;
        public float y;
        public float z;

        public Vector3_Serializable(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public Vector3_Serializable(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        public Vector3_Serializable(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
            z = 0;
        }

        public static implicit operator Vector3_Serializable(Vector3 vector)
        {
            return new Vector3_Serializable(vector);
        }

        public static implicit operator Vector3(Vector3_Serializable vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }

        public Vector3 ToUnserializable()
        {
            return new Vector3(x, y, z);
        }
    }
}
