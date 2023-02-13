using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;

public class TestItem : DynamicVisibilityRect
{
    [SerializeField] private Canvas canvas = null;
    public GameObject anotherLayout;

    public void Initialize()
    {
        Instantiate(anotherLayout, transform);
    }

    protected override void FirstBecameVisible()
    {
        Debug.LogError("FirstBecameVisible", gameObject);
        base.FirstBecameVisible();
    }

    protected override void BecameVisible()
    {
        canvas.enabled = true;
        Debug.LogError("BecameVisible", gameObject);
        base.BecameVisible();
    }

    protected override void BecameHidden()
    {
        canvas.enabled = false;
        Debug.LogError("BecameHidden", gameObject);
        base.BecameHidden();
    }
}
