using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    private Stack<TimerCallback> callbackPool = new Stack<TimerCallback>();
    private List<TimerCallback> activeCallbacks = new List<TimerCallback>();
    private readonly bool ApplicationIsQuitting;

    void Start()
    {
        // Schedule the ShowWelcomeMessage action to be called after 5 seconds
        ScheduleAction(5.0f, ShowWelcomeMessage);
    }

    void ShowWelcomeMessage()
    {
        Debug.Log("Welcome to The Ultimate Resort!");
    }

    void Update()
    {
        if (ApplicationIsQuitting) return;

        for (int i = activeCallbacks.Count - 1; i >= 0; i--)
        {
            var callback = activeCallbacks[i];
            callback.timeRemaining -= Time.deltaTime;

            if (callback.timeRemaining <= 0)
            {
                callback.action.Invoke();
                activeCallbacks.RemoveAt(i);
                ResetAndReturnToPool(callback);
            }
        }
    }

    public void ScheduleAction(float delay, Action action)
    {
        if (action == null)
        {
            Debug.LogError("Action to schedule is null.");
            return;
        }

        if (!IsActionScheduled(action))
        {
            TimerCallback callback = GetFromPool();
            callback.action = action;
            callback.timeRemaining = delay;
            activeCallbacks.Add(callback);
        }
    }

    private bool IsActionScheduled(Action action)
    {
        foreach (var callback in activeCallbacks)
        {
            if (callback.action == action)
            {
                return true;
            }
        }
        return false;
    }

    private TimerCallback GetFromPool()
    {
        if (callbackPool.Count > 0)
        {
            return callbackPool.Pop();
        }
        else
        {
            return new TimerCallback(); // Only allocate a new class if necessary
        }
    }

    private void ResetAndReturnToPool(TimerCallback callback)
    {
        callback.action = null; // Ensure the callback is reset before returning to pool
        callbackPool.Push(callback);
    }

    private class TimerCallback
    {
        public Action action;
        public float timeRemaining;
    }
}
