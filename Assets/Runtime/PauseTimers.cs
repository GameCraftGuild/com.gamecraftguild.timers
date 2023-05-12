using System.Collections;
using System.Collections.Generic;
using GameCraftGuild.Timers;
using UnityEngine;

public class PauseTimers : MonoBehaviour {

    Timer pausedTimer;
    Timer pauseControl1;
    Timer unpauseControl1;
    Timer pauseControl2;
    Timer unpauseControl2;

    Timer stoppedTimer;
    Timer stopControl;
    Timer startControl;

    Timer restartedTimer;
    Timer restartControl;




    // Start is called before the first frame update
    void Start () {
        pausedTimer = Timekeeper.AddTimer(7, () => DebugWithTime("Paused Timer Complete (@13s): "));
        pauseControl1 = Timekeeper.AddTimer(3, () => PauseTimer(pausedTimer));
        unpauseControl1 = Timekeeper.AddTimer(6, () => UnpauseTimer(pausedTimer));
        pauseControl2 = Timekeeper.AddTimer(9, () => PauseTimer(pausedTimer));
        unpauseControl2 = Timekeeper.AddTimer(12, () => UnpauseTimer(pausedTimer));

        stoppedTimer = Timekeeper.AddTimer(6, () => DebugWithTime("Stopped Timer Complete (@10s): "));
        stopControl = Timekeeper.AddTimer(2, () => StopTimer(stoppedTimer));
        startControl = Timekeeper.AddTimer(4, () => StartTimer(stoppedTimer));

        restartedTimer = Timekeeper.AddTimer(4, () => DebugWithTime("Restarted Timer Complete (@7s): "));
        restartControl = Timekeeper.AddTimer(3, () => RestartTimer(restartedTimer));

        Timekeeper.StartTimer(pausedTimer);
        Timekeeper.StartTimer(pauseControl1);
        Timekeeper.StartTimer(unpauseControl1);
        Timekeeper.StartTimer(pauseControl2);
        Timekeeper.StartTimer(unpauseControl2);

        Timekeeper.StartTimer(stoppedTimer);
        Timekeeper.StartTimer(stopControl);
        Timekeeper.StartTimer(startControl);

        Timekeeper.StartTimer(restartedTimer);
        Timekeeper.StartTimer(restartControl);
    }

    // Update is called once per frame
    void Update () {

    }

    private void DebugWithTime (string s) {
        Debug.Log(s + Time.time);
    }

    private void PauseTimer (Timer t) {
        Timekeeper.PauseTimer(t);
        DebugWithTime("Pausing at: ");
    }

    private void UnpauseTimer (Timer t) {
        Timekeeper.UnpauseTimer(t);
        DebugWithTime("Unpausing at: ");
    }

    private void StopTimer (Timer t) {
        Timekeeper.StopTimer(t);
    }

    private void StartTimer (Timer t) {
        Timekeeper.StartTimer(t);
    }

    private void RestartTimer (Timer t) {
        Timekeeper.RestartTimer(t);
    }

}
