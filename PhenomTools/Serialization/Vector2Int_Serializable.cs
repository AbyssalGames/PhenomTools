using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Vector2Int_Serializable
{
    public int x;
    public int y;

    public Vector2Int_Serializable(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
    }
    public Vector2Int_Serializable(Vector3Int vector)
    {
        x = vector.x;
        y = vector.y;
    }
    public Vector2Int_Serializable(Vector2Int vector)
    {
        x = vector.x;
        y = vector.y;
    }

    public Vector2Int ToUnserializable()
    {
        return new Vector2Int(x, y);
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y);
    }
}
