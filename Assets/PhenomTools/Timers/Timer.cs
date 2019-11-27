using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public event Action timerUpdatedEvent;
    public event Action timerCompleteEvent;

    public bool isRunning { get; private set; }
    public float currentTime { get; private set; }
    public float startTime { get; private set; }

    private float duration = 30f;
    private bool useSeconds = true; // Optimize to 1 per second vs 60 per second
    private AnimatorUpdateMode updateMode;

    public IEnumerator timerCoroutine;

    public Timer(float duration, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
    {
        StartTimer(duration, useSeconds, updateMode);
    }

    public void StartTimer(float duration, bool useSeconds = true, AnimatorUpdateMode updateMode = AnimatorUpdateMode.Normal)
    {
        if (isRunning)
            StopTimer();

        this.useSeconds = useSeconds;
        this.duration = duration;
        this.updateMode = updateMode;

        startTime = Time.realtimeSinceStartup;
        isRunning = true;
        currentTime = duration;

        TimerManager.RegisterNewTimer(this);

        timerUpdatedEvent?.Invoke();
    }

    public IEnumerator TimerCoroutine()
    {
        while (isRunning && currentTime > 0)
        {
            if (useSeconds)
            {
                if(updateMode == AnimatorUpdateMode.Normal || updateMode == AnimatorUpdateMode.AnimatePhysics)
                    yield return new WaitForSeconds(1);
                else if(updateMode == AnimatorUpdateMode.UnscaledTime)
                    yield return new WaitForSecondsRealtime(1);

                currentTime = (int)currentTime - 1;

                timerUpdatedEvent?.Invoke();
            }
            else
            {
                if (updateMode == AnimatorUpdateMode.UnscaledTime)
                {
                    currentTime = startTime + duration - Time.realtimeSinceStartup;
                }
                else
                {
                    if (updateMode == AnimatorUpdateMode.AnimatePhysics)
                        yield return new WaitForFixedUpdate();
                    else
                        yield return new WaitForEndOfFrame();

                    currentTime -= Time.deltaTime;
                }

                timerUpdatedEvent?.Invoke();
            }
        }

        currentTime = 0f;

        timerCoroutine = null;
        StopTimer();
    }

    public void ResetTimeLeft()
    {
        currentTime = duration;
        TimerManager.UpdateActiveTimersList();
    }

    public void StopTimer()
    {
        if (!isRunning)
            return;

        timerCompleteEvent?.Invoke();
        TimerEnded();
    }

    /// <summary>
    /// Stop the timer without notifying listeners of timerCompleteEvent
    /// </summary>
    public void KillTimer()
    {
        if (!isRunning)
            return;

        TimerEnded();
    }

    private void TimerEnded()
    {
        isRunning = false;
        TimerManager.RemoveTimer(this);

        if (timerCoroutine != null)
        {
            PhenomExtensions.coroutineHolder.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        if (timerCompleteEvent != null)
        { 
            Delegate[] invocationList = timerCompleteEvent.GetInvocationList();

            if (invocationList != null && invocationList.Length > 0)
            {
                foreach (Delegate d in invocationList)
                    timerCompleteEvent -= (Action)d;
            }
        }
    }
}
