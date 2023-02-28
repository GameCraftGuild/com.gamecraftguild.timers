namespace GameCraftGuild.Timers {

    /// <summary>
    /// Factory for creating timers.
    /// </summary>
    public class TimerFactory {

        /// <summary>
        /// Create a timer. The caller will be responsible for checking the time on the timer.
        /// </summary>
        /// <param name="duration">How long the timer should run for.</param>
        /// <param name="looping">Should the timer restart when it finishes.</param>
        /// <returns>The created timer.</returns>
        public static Timer CreateTimer (float duration, bool looping = false) {
            return new Timer(duration, looping);
        }

        /// <summary>
        /// Create a timer. The timer will be added to the global timekeeper to automatically check the time. If the timer is no longer in use, be sure to remove it from the timekeeper as well or the garbage collection will not know to dispose of it.
        /// </summary>
        /// <param name="duration">How long the timer should run for.</param>
        /// <param name="looping">Should the timer restart when it finishes.</param>
        /// <returns>The created timer.</returns>
        public static Timer CreateManagedTimer (float duration, bool looping = false) {
            Timer createdTimer = CreateTimer(duration, looping);

            Timekeeper.AddTimer(createdTimer);

            return createdTimer;
        }

    }
}