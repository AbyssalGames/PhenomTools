using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Vector3Int_Serializable
{
    public int x;
    public int y;
    public int z;

    public Vector3Int_Serializable(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public Vector3Int_Serializable(Vector3Int vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3Int_Serializable(Vector2Int vector)
    {
        x = vector.x;
        y = vector.y;
        z = 0;
    }

    public Vector3Int ToUnserializable()
    {
        return new Vector3Int(x, y, z);
    }
}