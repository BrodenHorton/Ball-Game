using UnityEngine;

public class Timer
{
    private float duration;
    private float elapsedTime;

    public Timer(float duration)
    {
        this.duration = duration;
        elapsedTime = 0f;
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public bool IsFinished()
    {
        return elapsedTime >= duration;
    }

    public void Reset()
    {
        elapsedTime = 0f;
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
