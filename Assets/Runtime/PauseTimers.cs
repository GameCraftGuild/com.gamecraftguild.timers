using System.Collections;
using System.Collections.Generic;
using GameCraftGuild.Timers;
using UnityEngine;

public class PauseTimers : MonoBehaviour {

    Timer pausedTimer;

    Timer stoppedTimer;

    Timer restartedTimer;

    Timer controlTimer;

    Timer startingTimer;

    // Start is called before the first frame update
    void Start () {
        pausedTimer = TimerFactory.CreateManagedTimer(10, true);
        pausedTimer.AddOnFinishAction(() => DebugWithTime("15s PausedTimer: "));

        stoppedTimer = TimerFactory.CreateManagedTimer(10, true);
        stoppedTimer.AddOnFinishAction(() => DebugWithTime("17s StoppedTimer: "));

        restartedTimer = TimerFactory.CreateManagedTimer(10, true);
        restartedTimer.AddOnFinishAction(() => DebugWithTime("13s RestartedTimer: "));

        controlTimer = TimerFactory.CreateManagedTimer(3, true);
        controlTimer.AddOnFinishAction(Stopping);

        // startingTimer = TimerFactory.CreateManagedTimer(4, false);
        // startingTimer.AddOnFinishAction(Starting);

        pausedTimer.StartTimer();
        stoppedTimer.StartTimer();
        restartedTimer.StartTimer();
        controlTimer.StartTimer();
    }

    // Update is called once per frame
    void Update () {

    }

    private void DebugWithTime (string s) {
        Debug.Log(s + Time.time);
    }

    private void Stopping () {
        DebugWithTime("3s Stopping: ");
        controlTimer.AddOnFinishAction(Starting);
        controlTimer.RemoveOnFinishAction(Stopping);
        controlTimer.SetDuration(4);

        pausedTimer.PauseTimer();
        stoppedTimer.StopTimer();
        restartedTimer.RestartTimer();
    }

    private void Starting () {
        DebugWithTime("7s Starting: ");
        controlTimer.AddOnFinishAction(SecondPause);
        controlTimer.RemoveOnFinishAction(Starting);
        controlTimer.SetDuration(1);

        pausedTimer.StartTimer();
        stoppedTimer.StartTimer();
    }

    private void SecondPause () {
        DebugWithTime("8s SecondPause: ");
        controlTimer.AddOnFinishAction(SecondStart);
        controlTimer.RemoveOnFinishAction(SecondPause);

        pausedTimer.PauseTimer();
    }

    private void SecondStart () {
        DebugWithTime("9s SecondStart: ");
        controlTimer.StopTimer();

        pausedTimer.StartTimer();
    }

}
