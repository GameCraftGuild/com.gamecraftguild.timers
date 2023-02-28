using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCraftGuild.Timers {

    /// <summary>
    /// Timer class with pausing and looping functionality.
    /// </summary>
    public class Timer {

        /// <summary>
        /// Time when the timer was started.
        /// </summary>
        private float startTime = 0;

        /// <summary>
        /// How long the timer runs for in seconds.
        /// </summary>
        private float duration = 0;

        /// <summary>
        /// How much time is saved from before the most recent pause.
        /// </summary>
        private float prePauseDuration = 0;

        /// <summary>
        /// If the timer currently running.
        /// </summary>
        private bool active = false;

        /// <summary>
        /// If the timer currently paused.
        /// </summary>
        private bool paused = false;

        /// <summary>
        /// If the timer should restart when it finishes.
        /// </summary>
        private bool looping = false;

        /// <summary>
        /// If onFinish actions are currently being called.
        /// </summary>
        private bool inOnFinish = false;

        /// <summary>
        /// List of actions to call when the timer has reached the duration.
        /// </summary>
        private List<Action> onFinish = new List<Action>();

        /// <summary>
        /// Actions to add to the onFinish list. This should be used in actions in onFinish to add new actions.
        /// </summary>
        private List<Action> toAddQueue = new List<Action>();

        /// <summary>
        /// Actions to remove from the onFinish list. This should be used in actions in onFinish to remove actions.
        /// </summary>
        private List<Action> toRemoveQueue = new List<Action>();

        /// <summary>
        /// Create a new timer.
        /// </summary>
        /// <param name="duration">How long the timer should run for in seconds.</param>
        /// <param name="looping">If the timer should restart once it finishes.</param>
        public Timer (float duration, bool looping = false) {
            this.duration = duration;
            this.looping = looping;
        }

        /// <summary>
        /// Check if the timer has reached the duration.
        /// </summary>
        public void CheckTime () {
            if (active) {
                if ((Time.time - startTime) + prePauseDuration >= duration) {
                    TimerFinished();
                }
            }
        }

        /// <summary>
        /// Called when the timer finishes. Calls action in onFinish and restarts the timer if looping is true.
        /// </summary>
        private void TimerFinished () {
            inOnFinish = true;
            onFinish.ForEach(action => action());
            inOnFinish = false;

            AddQueuedActions();
            RemoveQueuedActions();

            // Cannot call ResetTimer prior to this or stopping a looping timer in an onFinish action will not work properly.
            if (active && looping) {
                ResetTimer();
                StartTimer();
            } else {
                ResetTimer();
            }
        }

        /// <summary>
        /// Add the queued actions to onFinish.
        /// </summary>
        private void AddQueuedActions () {
            toAddQueue.ForEach(action => onFinish.Add(action));
            toAddQueue.Clear();
        }

        /// <summary>
        /// Remove the queued actions to onFinish.
        /// </summary>
        private void RemoveQueuedActions () {
            toRemoveQueue.ForEach(action => onFinish.Remove(action));
            toRemoveQueue.Clear();
        }

        /// <summary>
        /// If the timer is not currently running, start the timer. If the timer is paused, resumes at the time when the timer was paused.
        /// </summary>
        public void StartTimer () {
            if (active) {
                return;
            }

            if (!paused) {
                ResetTimer();
            } else {
                paused = false;
            }

            startTime = Time.time;
            active = true;
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void StopTimer () {
            active = false;
            paused = false;
        }

        /// <summary>
        /// Pause the timer. When StartTimer is called, the timer will resume from the current time.
        /// </summary>
        public void PauseTimer () {
            prePauseDuration += Time.time - startTime;
            active = false;
            paused = true;
        }

        /// <summary>
        /// Restart the timer. Identical to calling StopTimer immediately followed by StartTimer.
        /// </summary>
        public void RestartTimer () {
            StopTimer();
            StartTimer();
        }

        /// <summary>
        /// Reset the timer. Also effectively stops the timer.
        /// </summary>
        private void ResetTimer () {
            startTime = 0;
            prePauseDuration = 0;
            active = false;
            paused = false;
        }

        /// <summary>
        /// Get a percentage of how complete the timer is. Useful for lerps or similar functions. Divides the current time by the timer's duration.
        /// </summary>
        /// <returns>The percent completion of the timer. This is the current time divided by the timer's duration.</returns>
        public float GetPercentageComplete () {
            return GetCurrentTime() / duration;
        }

        /// <summary>
        /// Get the current time for the timer in seconds.
        /// </summary>
        /// <returns>The current time of the timer in seconds counting up from when the timer was started. A stopped (and not paused) timer returns 0; a paused timer returns the amount of time when the timer was paused.</returns>
        public float GetCurrentTime () {
            if (!active && !paused) {
                return 0;
            }

            if (paused) {
                return prePauseDuration;
            }

            return (Time.time - startTime) + prePauseDuration;
        }

        /// <summary>
        /// Get the duration of the timer.
        /// </summary>
        /// <returns>How long the timer runs for (in seconds).</returns>
        public float GetDuration () {
            return duration;
        }

        /// <summary>
        /// Set the duration of the timer.
        /// </summary>
        /// <param name="newDuration">How long the timer should run for (in seconds).</param>
        public void SetDuration (float newDuration) {
            duration = newDuration;
        }

        /// <summary>
        /// Should the timer restart when it finishes.
        /// </summary>
        /// <returns>If the timer should restart when it finishes.</returns>
        public bool IsLooping () {
            return looping;
        }

        /// <summary>
        /// Set if the timer should restart when it finishes.
        /// </summary>
        /// <param name="shouldLoop">If the timer should restart when it finishes.</param>
        public void SetLooping (bool shouldLoop) {
            looping = shouldLoop;
        }

        /// <summary>
        /// Add an action to be called when the timer finishes. NOTE: If in an action in onFinish, the action will be queued to be added when the onFinish action finish. Queued adds will be done before queued removes.
        /// </summary>
        /// <param name="actionToAdd">Action to be called when the timer finishes.</param>
        public void AddOnFinishAction (Action actionToAdd) {
            if (inOnFinish) {
                toAddQueue.Add(actionToAdd);
            } else {
                onFinish.Add(actionToAdd);
            }
        }

        /// <summary>
        /// Stop an action from being called when the timer finishes. NOTE: If in an action in onFinish, the action will be queued to be removed when the onFinish actions finish. Queued adds will be done before queued removes.
        /// </summary>
        /// <param name="actionToRemove">Action to no longer be called when the timer finishes.</param>
        /// <returns>If <paramref name="actionToRemove" /> was removed successfully or if the action was added the remove queue.</returns>
        public bool RemoveOnFinishAction (Action actionToRemove) {
            if (inOnFinish) {
                toRemoveQueue.Add(actionToRemove);
                return true;
            } else {
                return onFinish.Remove(actionToRemove);
            }
        }

    }
}
