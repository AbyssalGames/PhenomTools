using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TimerManager
{
    public static List<Timer> activeTimers { get; private set; } = new List<Timer>();

    public static void RegisterNewTimer(Timer timer)
    {
        timer.timerCoroutine = timer.TimerCoroutine();
        PhenomExtensions.coroutineHolder.StartCoroutine(timer.timerCoroutine);

        if(!activeTimers.Contains(timer))
            activeTimers.Add(timer);

        UpdateActiveTimersList();
    }

    public static void RemoveTimer(Timer timer)
    {
        if (timer.isRunning)
            timer.StopTimer();

        activeTimers.Remove(timer);
    }

    public static void UpdateActiveTimersList()
    {
        activeTimers.OrderBy(t => t.currentTime);
    }
}
