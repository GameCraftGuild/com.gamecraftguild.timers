using System.Collections;
using System.Collections.Generic;
using GameCraftGuild.Timers;
using UnityEngine;

public class MakeTimers : MonoBehaviour {

    Timer t2;

    Timer t1;

    Timer t5;

    Timer t10;

    Timer t15;

    // Start is called before the first frame update
    void Start () {
        t1 = TimerFactory.CreateManagedTimer(1, true);
        t1.AddOnFinishAction(EverySecond);

        t5 = TimerFactory.CreateManagedTimer(5, true);
        t5.AddOnFinishAction(EveryFiveSeconds);

        t10 = TimerFactory.CreateManagedTimer(10, false);
        t10.AddOnFinishAction(OnceOffTenSeconds);

        t15 = TimerFactory.CreateManagedTimer(15, false);
        t15.AddOnFinishAction(OnceOffFifteenSeconds);

        t2 = TimerFactory.CreateTimer(2, true);

        t1.StartTimer();
        t5.StartTimer();
        t10.StartTimer();
        t15.StartTimer();
        t2.StartTimer();
    }

    // Update is called once per frame
    void Update () {

    }

    void FixedUpdate () {
        t2.CheckTime();
    }

    void OnDisable () {
        Timekeeper.RemoveTimer(t1);
    }

    public void EverySecond () {
        Debug.Log("1s - Current Time: " + Time.time);
    }

    public void EveryOtherSecond () {
        Debug.Log("2s - Current Time: " + Time.time);
    }

    public void EveryFiveSeconds () {
        Debug.Log("5s - Current Time: " + Time.time);
    }

    public void OnceOffTenSeconds () {
        Debug.Log("10s - Once Off: " + Time.time);
    }

    public void OnceOffFifteenSeconds () {
        Debug.Log("15s - Once Off: " + Time.time);
    }
}
