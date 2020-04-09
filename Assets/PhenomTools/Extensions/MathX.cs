using DG.Tweening;
using DG.Tweening.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (i == 1)
            return true;
        else
            return false;
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
        if (i % 2 == 0)
            return true;
        else
            return false;
    }

    public static int Loop(this int i, int m)
    {
        int n = i % m;
        if (n < 0)
            n = m + i;

        return n;
    }

    public static int[] SplitToEqualParts(this int number, int parts)
    {
        if (parts > number)
        {
            Debug.LogWarning(string.Concat("Cannot split: ", number, " into: ", parts, " parts"));
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

    public static string ToFixedLengthString(this int i, int length)
    {
        int iLength = i.ToString().Length;

        if (iLength < length)
        {
            string newString = "";
            for (int n = 0; n < length - iLength; n++)
            {
                string.Concat(newString, "0");
            }

            string.Concat(newString, i.ToString());

            return newString;
        }
        else if (iLength > length)
        {
            string s = (i / Mathf.Pow(10, iLength)).ToString("F" + length.ToString());
            return s.Remove(0, 2); // remove the 0 and decimal point
        }
        else
        {
            return i.ToString();
        }
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
        float n = i % max;
        if (n < min)
            n = min + i;

        return n;
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
        else
        {
            return (float)Math.Round(number / multiple, roundingType) * multiple;
        }
    }

    public static Tweener Roll(this float i, DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
    {
        DOTween.Kill(i);
        return DOTween.To(getter, setter, endValue, duration).SetAs(tweenParams_Sine);
    }
    #endregion


    

}
