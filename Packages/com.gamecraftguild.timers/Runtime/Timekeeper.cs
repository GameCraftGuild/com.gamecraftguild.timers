using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCraftGuild.Timers {

    /// <summary>
    /// Timekeeper to check the times on timers. Persists between scenes.
    /// </summary>
    public class Timekeeper : MonoBehaviour {

        private struct TimerInfo {
            public bool Loop;
            public bool UseUnscaledTime;
            public Action OnFinish;
        }

        private static Dictionary<Timer, TimerInfo> _timerInfoByTimer = new Dictionary<Timer, TimerInfo>();

        private static Timer stopCurrentLoop;

        private static bool checkingTime;

        private static List<Timer> timersToRemove = new List<Timer>();

        /// <summary>
        /// Static reference to the single timekeeper instance.
        /// </summary>
        private static Timekeeper instance;

        /// <summary>
        /// Add a timer to the timekeeper.
        /// </summary>
        /// <param name="duration">Duration for the timer.</param>
        /// <param name="loop">Should the timer loop.</param>
        /// <param name="onFinish">Action to perform when the timer finishes.</param>
        /// <returns>The timer.</returns>
        public static Timer AddTimer(float duration, Action onFinish, bool loop = false, bool useUnscaledTime = false) {
            if (instance == null) {
                Initialize();
            }

            Timer timer = new Timer(duration);
            _timerInfoByTimer.Add(timer, new TimerInfo() { Loop = loop, UseUnscaledTime = useUnscaledTime, OnFinish = onFinish });
            return timer;
        }

        /// <summary>
        /// Start the <paramref name="timer" />.
        /// </summary>
        /// <param name="timer">Timer to start.</param>
        public static void StartTimer(Timer timer) {
            timer.Start(GetTimeType(timer));
        }

        /// <summary>
        /// Stop the <paramref name="timer" />. If this is called during the OnFinish action of a timer where Loop == true, it will prevent the timer from restarting.
        /// </summary>
        /// <param name="timer">Timer to stop.</param>
        public static void StopTimer(Timer timer) {
            timer.Stop();
            stopCurrentLoop = timer;
        }

        /// <summary>
        /// Restart the <paramref name="timer" />.
        /// </summary>
        /// <param name="timer">Timer to restart.</param>
        public static void RestartTimer(Timer timer) {
            timer.Restart(GetTimeType(timer));
        }

        /// <summary>
        /// Pause the <paramref name="timer" />.
        /// </summary>
        /// <param name="timer">Timer to pause.</param>
        public static void PauseTimer(Timer timer) {
            timer.Pause(GetTimeType(timer));
        }

        /// <summary>
        /// Unpause the <paramref name="timer" />.
        /// </summary>
        /// <param name="timer">Timer to unpause.</param>
        public static void UnpauseTimer(Timer timer) {
            timer.Unpause(GetTimeType(timer));
        }

        /// <summary>
        /// Get the percentage complete for the timer. <paramref name="timer" />.
        /// </summary>
        /// <param name="timer">Timer to get the percent complete for.</param>
        /// <returns>The percentage complete for the timer as a float between 0 and 1 inclusive.</returns>
        public static float GetPercentComplete(Timer timer) {
            return timer.PercentComplete(GetTimeType(timer));
        }

        /// <summary>
        /// Get either the time or the unscaled time based on the <paramref name="timer" />'s TimerInfo.
        /// </summary>
        /// <param name="timer">The timer to get the time for.</param>
        /// <returns>Either the time or unscaled time based on <paramref name="timer" />.</returns>
        private static float GetTimeType(Timer timer) {
            return _timerInfoByTimer[timer].UseUnscaledTime ? Time.unscaledTime : Time.time;
        }

        /// <summary>
        /// Remove a timer from the timekeeper. If this is called during an OnFinish action, the timer will be queued to be removed once all time checks have completed.
        /// </summary>
        /// <param name="timerToRemove">Timer to remove from the timekeeper.</param>
        public static void RemoveTimer(Timer timerToRemove) {
            if (!checkingTime) {
                _timerInfoByTimer.Remove(timerToRemove);
            } else {
                timersToRemove.Add(timerToRemove);
            }
        }

        /// <summary>
        /// Create an instance of the timekeeper.
        /// </summary>
        public static void Initialize() {
            if (instance != null) return;
            GameObject tempGameObject = new GameObject();
            tempGameObject.name = "TimeKeeper";
            tempGameObject.AddComponent<Timekeeper>();
            DontDestroyOnLoad(tempGameObject);

            instance = tempGameObject.GetComponent<Timekeeper>();
        }

        /// <summary>
        /// Remove timers that were queued to be removed.
        /// </summary>
        private static void RemoveQueuedTimers () {
            foreach (Timer t in timersToRemove) RemoveTimer(t);

            timersToRemove.Clear();
        }

        /// <summary>
        /// Called at a fixed time interval. Checks the time on all the timers. Timers are checked in the order they have been added.
        /// </summary>
        private void FixedUpdate() {
            checkingTime = true;
            foreach (KeyValuePair<Timer, TimerInfo> timer in _timerInfoByTimer) {
                stopCurrentLoop = null;
                if (timer.Key.IsComplete(GetTimeType(timer.Key))) {

                    timer.Value.OnFinish();

                    if (timer.Value.Loop && stopCurrentLoop != timer.Key) { // if the current timer was stopped in OnFinish, don't restart it.
                        timer.Key.Restart(GetTimeType(timer.Key));
                    } else {
                        timer.Key.Stop();
                    }

                }
            }
            checkingTime = false;
            RemoveQueuedTimers();
        }

    }
}
