using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private float unscaledTimePassed;
    private float timePassed;
    public UnityEvent TimerEvent { get; private set; }
    public UnityEvent UnscaledTimerEvent { get; private set; }


    void Update()
    {
        unscaledTimePassed +=Time.unscaledDeltaTime;
        timePassed +=timePassed;
        if (unscaledTimePassed >= 1)
        {
            unscaledTimePassed -= 1;
            UnscaledTimerEvent.Invoke();
        }
        if (timePassed >= 1)
        {
            timePassed -= 1;
            UnscaledTimerEvent.Invoke();
        }
    }
}
