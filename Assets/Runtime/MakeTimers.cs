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

        t1 = Timekeeper.AddTimer(1, EverySecond, true);
        t5 = Timekeeper.AddTimer(5, EveryFiveSeconds, true);
        t10 = Timekeeper.AddTimer(10, OnceOffTenSeconds);
        t15 = Timekeeper.AddTimer(15, OnceOffFifteenSeconds);

        Timekeeper.StartTimer(t1);
        Timekeeper.StartTimer(t5);
        Timekeeper.StartTimer(t10);
        Timekeeper.StartTimer(t15);

    }

    // Update is called once per frame
    void Update () {

    }

    void OnDestroy () {
        Timekeeper.RemoveTimer(t1);
        Timekeeper.RemoveTimer(t5);
        Timekeeper.RemoveTimer(t10);
        Timekeeper.RemoveTimer(t15);
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
