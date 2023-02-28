using System.Collections.Generic;
using UnityEngine;

namespace GameCraftGuild.Timers {

    /// <summary>
    /// Timekeeper to check the times on timers. Persists between scenes.
    /// </summary>
    public class Timekeeper : MonoBehaviour {

        /// <summary>
        /// List of all the timers to be checked.
        /// </summary>
        private static List<Timer> allTimers = new List<Timer>();

        /// <summary>
        /// Static reference to the single timekeeper instance.
        /// </summary>
        private static Timekeeper instance;

        /// <summary>
        /// Add a timer to the timekeeper.
        /// </summary>
        /// <param name="timerToAdd">Timer to add to the timekeeper.</param>
        public static void AddTimer (Timer timerToAdd) {
            if (instance == null) {
                CreateTimekeeper();
            }
            allTimers.Add(timerToAdd);
        }

        /// <summary>
        /// Remove a timer from the timekeeper.
        /// </summary>
        /// <param name="timerToRemove">Timer to remove from the timekeeper.</param>
        /// <returns>If <paramref name="timerToRemove" /> was removed successfully.</returns>
        public static bool RemoveTimer (Timer timerToRemove) {
            return allTimers.Remove(timerToRemove);
        }

        /// <summary>
        /// Create an instance of the timekeeper.
        /// </summary>
        private static void CreateTimekeeper () {
            GameObject tempGameObject = new GameObject();
            tempGameObject.name = "TimeKeeper";
            tempGameObject.AddComponent<Timekeeper>();
            DontDestroyOnLoad(tempGameObject);

            instance = tempGameObject.GetComponent<Timekeeper>();
        }

        /// <summary>
        /// Called at a fixed time interval. Checks the time on all the timers.
        /// </summary>
        private void FixedUpdate () {
            allTimers.ForEach(timer => timer.CheckTime());
        }

    }
}
