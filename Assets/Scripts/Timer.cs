using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    public event Action OnTimerEnded;

    private readonly MonoBehaviour _monoBeh;
    private float _seconds;

    public Timer(MonoBehaviour monoBeh)
    {
        _monoBeh = monoBeh;
    }

    public void StartTimer(float seconds)
    {
        _seconds = seconds;
        _monoBeh.StartCoroutine(Time());
    }

    private IEnumerator Time()
    {
        yield return new WaitForSeconds(_seconds);
        OnTimerEnded?.Invoke();
    }
}
