using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class PhenomExtensions
{
    public static Vector2Int ToTilePosition(this Vector3 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
    public static Vector2Int ToTilePosition(this Vector2 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
    }
    public static Vector2Int ToChunk(this Vector2Int vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x / 16f), Mathf.FloorToInt(vector.y / 16f));
    }
    public static Vector2Int ToChunk(this Vector3 vector)
    {
        return new Vector2Int(Mathf.FloorToInt(vector.x / 16f), Mathf.FloorToInt(vector.y / 16f));
    }
}
