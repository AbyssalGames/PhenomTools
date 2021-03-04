﻿using UnityEngine;

public class SafeZone : MonoBehaviour
{
    //public static float trueWidth;
    //public static float trueHeight;
    public static float width;
    public static float height;

    RectTransform Panel;
    //Rect LastSafeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        Panel = GetComponent<RectTransform>();
        Refresh();
    }

    //void Update()
    //{
    //    Refresh();
    //}

    void Refresh()
    {
        Rect safeArea = GetSafeArea();

        //if (safeArea != LastSafeArea)
            ApplySafeArea(safeArea);
    }

    Rect GetSafeArea()
    {
        return Screen.safeArea;
    }

    void ApplySafeArea(Rect r)
    {
        //LastSafeArea = r;

        width = r.width;
        height = r.height;

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = r.position;
        Vector2 anchorMax = r.position + r.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        Panel.anchorMin = anchorMin;
        Panel.anchorMax = anchorMax;

        //width = Panel.sizeDelta.x;
        //height = Panel.sizeDelta.y;

        //Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
        //    name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
    }
}