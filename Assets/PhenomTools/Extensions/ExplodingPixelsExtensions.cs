using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ExPix
namespace PhenomTools
{
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

#region Item Objects

        public static Vector3 ToVector3(this ItemObject itemObject)
        {
            return new Vector3(itemObject.id, itemObject.seed, itemObject.level);
        }

        public static ItemObject ToItemObject(this Vector3 vector)
        {
            return new ItemObject {id = (int) vector.x, seed = (int) vector.y, level = (int) vector.z};
        }

        public static bool IsEqual(this ItemObject item, ItemObject other)
        {
            return item.id == other.id && item.seed == other.seed && item.level == other.level;
        }

        public static bool IsEqual(this ItemObject item, Vector3 other)
        {
            return item.id == (int) other.x && item.seed == (int) other.y && item.level == (int) other.z;
        }

        public static void Set(this ItemObject item, ItemObject other)
        {
            item.id = other.id;
            item.seed = other.seed;
            item.level = other.level;
        }

        public static void Set(this ItemObject item, Vector3 other)
        {
            item.id = (int) other.x;
            item.seed = (int) other.y;
            item.level = (int) other.z;
        }

        public static void Clear(this ItemObject item)
        {
            item.id = 0;
            item.seed = 0;
            item.level = 0;
        }

        public static ItemObject Clone(this ItemObject item)
        {
            return new ItemObject
            {
                id = item.id,
                seed = item.seed,
                level = item.level
            };
        }

        //public static IContainerCommandInput Initialize(this IContainerCommandInput i)
        //{
        //    i.remove = -1;
        //    i.move = -1;
        //    i.toSlot = -1;
        //    return i;
        //}

#endregion
    }
}
#endif