using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;

namespace PhenomTools.Utility
{
  public static class MathX
  {
    private static readonly TweenParams tweenParams_Sine = new TweenParams().SetEase(Ease.InOutSine);

    #region Integer

    public static int Remap(this int oldValue, int oldMin, int oldMax, int newMin, int newMax)
    {
      float floatValue = oldValue;
      return Mathf.RoundToInt(floatValue.Remap(oldMin, oldMax, newMin, newMax));
    }

    public static float ToFloat(this int value)
    {
      return value;
    }

    public static bool ToBoolBinary(this int i)
    {
      return i == 1;
    }

    public static bool[] ToBoolBinary(this int[] intArray)
    {
      bool[] boolArray = new bool[intArray.Length];

      for (int i = 0; i < intArray.Length; i++)
      {
        boolArray[i] = intArray[i].ToBoolBinary();
      }

      return boolArray;
    }

    public static bool ToBoolEvenOdd(this int i)
    {
      return i % 2 == 0;
    }

    public static int Loop(this int i, int max, int min = 0)
    {
      int n = i % max;
      if (n < min)
        n = max;
      else if (n > max)
        n = min;

      return n;
    }

    public static int[] SplitToEqualParts(this int number, int parts)
    {
      if (parts > number)
      {
        Debug.LogWarningFormat("Cannot split: {0} into: {1} parts", number, parts);
        return null;
      }

      int remainder = number % parts;
      int floorNumber = Mathf.FloorToInt(number / (float)parts);

      int[] split = new int[parts];

      for (int p = 0; p < parts; p++)
      {
        split[p] = floorNumber;
        if (p < remainder)
          split[p]++;
      }

      return split;
    }

    public static int Round(this int number, int multiple, MidpointRounding roundingType = MidpointRounding.AwayFromZero)
    {
      return (int)Math.Round(number / (double)multiple, roundingType) * multiple;
    }

    public static int Ceil(this int number, int multiple)
    {
      return Mathf.CeilToInt(number / (float)multiple) * multiple;
    }

    public static int Floor(this int number, int multiple)
    {
      return Mathf.FloorToInt(number / (float)multiple) * multiple;
    }

    public static Tweener Roll(this int i, DOGetter<int> getter, DOSetter<int> setter, int endValue, float duration)
    {
      DOTween.Kill(i);
      return DOTween.To(getter, setter, endValue, duration).SetAs(tweenParams_Sine);
    }

    public static int Invert(this int i, int max, int min = 0)
    {
      if (i < min) i = min;
      else if (i > max) i = max;

      return (max + min) - i;
    }

    #endregion

    #region Float

    public static float Remap(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
      return ((oldValue - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
    }

    public static float RemapInverse(this float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
      return -(-oldValue).Remap(-oldMax, -oldMin, -newMin, -newMax);
    }

    public static float RemapNormalized(this float oldValue, float oldMin, float oldMax)
    {
      return (oldValue - oldMin) * 1 / (oldMax - oldMin);
    }

    public static float RemapInverseNormalized(this float oldValue, float oldMin, float oldMax)
    {
      return -(-oldValue).Remap(-oldMax, -oldMin, 0, -1);
    }

    public static bool ToBool(this float value)
    {
      return value >= 0;
    }

    public static float Loop(this float i, float min, float max)
    {
      if (min >= max)
        return 0;

      if (i < min)
        return max - Math.Abs((i - min) % (max - min));
      
      return (i - min) % (max - min) + min;
    }

    public static float Round(this float number, float multiple = 1f, MidpointRounding roundingType = MidpointRounding.AwayFromZero)
    {
      if (multiple < 1)
      {
        float i = (float)Math.Floor(number);
        float x2 = i;
        while ((x2 += multiple) < number) ;
        float x1 = x2 - multiple;
        return (Math.Abs(number - x1) < Math.Abs(number - x2)) ? x1 : x2;
      }

      return (float)Math.Round(number / multiple, roundingType) * multiple;
    }

    public static Tweener Roll(this float i, DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
    {
      DOTween.Kill(i);
      return DOTween.To(getter, setter, endValue, duration).SetAs(tweenParams_Sine);
    }

    public static float ToFeet(this float meters, bool precise = false)
    {
      return meters * (precise ? 3.28084f : 3f);
    }

    public static float ToMeters(this float feet, bool precise = false)
    {
      return feet / (precise ? 3.28084f : 3f);
    }

    #endregion

    #region Vector

    public static Vector3Int RoundToInt(this Vector3 vector)
    {
      return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    public static Vector3Int ToVector3Int(this Vector2Int vector)
    {
      return new Vector3Int(vector.x, vector.y, 0);
    }

    public static Vector2Int ToVector2Int(this Vector3Int vector)
    {
      return new Vector2Int(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector3Int vector)
    {
      return new Vector3(vector.x, vector.y, vector.z);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
      return new Vector3(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2Int vector)
    {
      return new Vector3(vector.x, vector.y);
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
      return new Vector2(vector.x, vector.y);
    }

    #endregion
  }
}