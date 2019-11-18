using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum CardinalDirection
{
    North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest
}

public static class PhenomUtils
{
    public static string GetCurrentPlatformString()
    {
        return GetPlatformString(Application.platform);
    }
    public static string GetPlatformString(RuntimePlatform platform)
    {
        switch (platform)
        {
            default:
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";
        }
    }

    /// <summary>Gets a string from a text file at the specified path.</summary>
    /// <param name="path">The path to the file.</param>
    public static string GetStringFromFile (string path)
    {
        if (!File.Exists(path))
            throw new System.Exception("File does not exist at specified path");

        StreamReader reader = new StreamReader(path);
        string text = reader.ReadToEnd();
        reader.Close();

        return text;
    }

    public static int GetDeviceOrientation(bool includeFaceUp = false, bool includeFaceDown = false)
    {
        switch (Input.deviceOrientation)
        {
            default:
            case DeviceOrientation.Unknown:
            case DeviceOrientation.Portrait:
                return 0;
            case DeviceOrientation.LandscapeLeft:
                return 1;
            case DeviceOrientation.LandscapeRight:
                return 2;
            case DeviceOrientation.PortraitUpsideDown:
                return 3;
            case DeviceOrientation.FaceUp:
                return includeFaceUp ? 4 : 0;
            case DeviceOrientation.FaceDown:
                return includeFaceDown ? 5 : 0;
        }
    }

    public static string ConvertIntToFixedLengthString(int i, int length)
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

    public static Vector2 CardinalDirectionToVector(CardinalDirection cardinalDirection, bool normalized = false)
    {
        switch (cardinalDirection)
        {
            default:
            case CardinalDirection.North:
                return Vector2.up;
            case CardinalDirection.NorthEast:
                return normalized ? Vector2.one.normalized : Vector2.one;
            case CardinalDirection.East:
                return Vector2.right;
            case CardinalDirection.SouthEast:
                return normalized ? new Vector2(1, -1).normalized : new Vector2(1, -1);
            case CardinalDirection.South:
                return Vector2.down;
            case CardinalDirection.SouthWest:
                return normalized ? -Vector2.one.normalized : -Vector2.one;
            case CardinalDirection.West:
                return Vector2.left;
            case CardinalDirection.NorthWest:
                return normalized ? new Vector2(-1,1).normalized : new Vector2(-1, 1);
        }
    }

    public static int CardinalDirectionToQuadrant(CardinalDirection cardinalDirection)
    {
        switch (cardinalDirection)
        {
            default:
            case CardinalDirection.North:
            case CardinalDirection.NorthEast:
                return 0;
            case CardinalDirection.East:
            case CardinalDirection.SouthEast:
                return 1;
            case CardinalDirection.South:
            case CardinalDirection.SouthWest:
                return 2;
            case CardinalDirection.West:
            case CardinalDirection.NorthWest:
                return 3;
        }
    }

    public static int GetDirectionOfRelativeObject(Vector3 baseObjectPos, Vector3 baseObjectAxis, Vector3 otherObjectPos)
    {
        Vector3 toOther = otherObjectPos - baseObjectPos;

        if (Vector3.Dot(baseObjectAxis, toOther) > 0)
            return -1;
        else
            return 1;
    }

    public static int GetWeightedValue(int value, int[] finalValues, int[] weights)
    {
        int[] weightIndex = new int[finalValues.Length];

        int finalNumber = 0;
        int counter = 0;

        for (int i = 0; i < finalValues.Length; i++)
        {
            counter += weights[i];
            weightIndex[i] = counter;
        }

        for (int i = 0; i < finalValues.Length; i++)
        {
            if (value < weightIndex[i])
            {
                finalNumber = finalValues[i];
                break;
            }
        }

        return finalNumber;
    }

    public static void DelayActionByTime(float time, Action callback)
    {
        PhenomExtensions.coroutineHolder.StartCoroutine(DelayActionByTimeCoroutine(time, callback));
    }
    private static IEnumerator DelayActionByTimeCoroutine(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public static void DelayActionByFrames(int frames, Action callback)
    {
        PhenomExtensions.coroutineHolder.StartCoroutine(DelayActionByFramesCoroutine(frames, callback));
    }
    private static IEnumerator DelayActionByFramesCoroutine(int frames, Action callback)
    {
        int count = 0;

        while (count < frames)
        {
            yield return null;
            count++;
        }

        callback();
    }

    public static IEnumerator RepeatAction(float time, int count, Action callback1, Action callback2)
    {
        WaitForSeconds waitDuration = new WaitForSeconds(time);
        while (count > 0)
        {
            callback1();
            count--;
            yield return waitDuration;
        }

        callback2();
    }
}
