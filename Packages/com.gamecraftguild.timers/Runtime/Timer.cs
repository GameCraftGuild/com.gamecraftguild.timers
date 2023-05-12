namespace GameCraftGuild.Timers {

    public class Timer {
        private float _durationMs;
        private float _completeTimeMs;
        private float _pausedTimeMs;
        private bool _started = false;
        private bool _paused = false;

        public Timer (float durationMs) {
            _durationMs = durationMs;
        }

        public bool IsComplete (float currentTime) {
            return _started && !_paused && currentTime >= _completeTimeMs;
        }

        public void Start (float currentTime) {
            if (_started) return;

            _started = true;
            _completeTimeMs = currentTime + _durationMs;
        }

        public void Stop () {
            _started = false;
            _paused = false;
        }

        public void Restart (float currentTime) {
            Stop();
            Start(currentTime);
        }

        public void Pause (float currentTime) {
            if (_paused) return;

            _paused = true;
            _pausedTimeMs = currentTime;

        }

        public void Unpause (float currentTime) {
            if (!_paused) return;

            _paused = false;
            float durationPausedMs = _pausedTimeMs - currentTime;
            _completeTimeMs += durationPausedMs;
        }

        public float PercentComplete (float currentTime) {
            if (!_started) return 0f;
            if (_paused) return (_durationMs - (_completeTimeMs - _pausedTimeMs)) / _durationMs;

            return (_durationMs - (_completeTimeMs - currentTime)) / _durationMs;
        }
    }
}