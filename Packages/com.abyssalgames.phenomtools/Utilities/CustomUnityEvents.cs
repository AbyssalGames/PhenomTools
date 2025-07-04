﻿using System;

namespace UnityEngine.Events
{
    [Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

    [Serializable]
    public class UnityEventInt : UnityEvent<int> { }

    [Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    [Serializable]
    public class UnityEventGameObject : UnityEvent<GameObject> { }

    [Serializable]
    public class UnityEventColor : UnityEvent<Color> { }
}
