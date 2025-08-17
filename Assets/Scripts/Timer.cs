using System;
using UnityEngine;

public class Timer
{
    private float duration;
    private float elapsedTime;
    private bool emittedAction = false;
    public Action finished;
    public Timer(float duration)
    {
        this.duration = duration;
        elapsedTime = 0f;
    }

    public Timer (float duration, Action callback) : this(duration)
    {
       finished += callback;
    }

    public void Update()
    {
        if (!emittedAction && IsFinished())
        {
            emittedAction = true;
            finished?.Invoke();
        }
        else if (!IsFinished())
            elapsedTime += Time.deltaTime;
    }

    public bool IsFinished()
    {
        return elapsedTime >= duration;
    }

    public void Reset()
    {
        elapsedTime = 0f;
        emittedAction = false;
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
    public void SetFinished()
    {
        elapsedTime = duration;
    }
}
