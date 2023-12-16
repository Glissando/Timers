namespace GameTime
{
    public class Timer
    {
        float _elapsedTime = 0f; //The time passed for the current loop
        readonly float _totalTime = 0f; //the total time that must be passed for a loop
        readonly float _delay; //The amount of delay before the timer can start
        float _delayTime;
        readonly int _repeatCount; //How many iterations must this timer go through if it loops
        public float duration = 0.0f;
        /// <summary>
        /// How many times has this Timer Completed. This will have a maximum of 1 if the timer is not repeatable.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Has the timer finished executed.
        /// </summary>
        public bool IsFinished { get; private set; }
        /// <summary>
        /// Determines whether the timer is currently paused, meaning it will not be updated.
        /// </summary>
        public bool IsPaused { get; set; }
        /// <summary>
        /// The percentage within the range from 0 to 1 that the timer is to finishing a current iteraiton.
        /// </summary>
        public float PercentageDone { get => _elapsedTime / _totalTime; }
        /// <summary>
        /// The total elapsed time that the timer has gone through while running.
        /// </summary>
        public float ElapsedTime { get => _elapsedTime; }
        public float TimeLeft { get => duration - _elapsedTime; }
        Action _callback = delegate { };

        /// <summary>
        /// An event that is fired each time the timer repeats.
        /// </summary>
        public Action OnLoop = delegate { };
        /// <summary>
        /// An event that is fired when the timer is finished.
        /// </summary>
        public Action OnFinish = delegate { };
        /// <summary>
        /// An event that is fired when the timer starts.
        /// </summary>
        public Action OnStart = delegate { };

        public Timer(float duration)
        {
            _totalTime = duration;
        }

        public Timer(float duration, Action callback) : this(callback, duration)
        {

        }

        public Timer(float duration, int repeatCount, Action callback) : this(callback, duration, repeatCount)
        {

        }

        public Timer(float duration, int repeatCount, float delay, Action callback) : this(callback, duration, repeatCount, delay)
        {

        }

        private Timer(Action callback, float duration, int repeatCount = 0, float delay = 0.0f)
        {
#if DEBUG
            if (delay < 0f)
            {
                throw new ArgumentOutOfRangeException("Delay can not be negative.");
            }
            if (duration < 0f)
            {
                throw new ArgumentOutOfRangeException("Duration can not be negative.");
            }
#endif
            _totalTime = duration;
            _repeatCount = repeatCount;
            this._delay = delay;
            _callback = callback;
        }

        public void Update(float deltaTime)
        {
            if (_delay > 0 && _delayTime < _delay)
            {
                _delayTime += deltaTime;
                duration = _delayTime + _totalTime;
                return;
            }

            if (_elapsedTime == 0 && Count == 0)
            {
                OnStart?.Invoke();
            }

            _elapsedTime += deltaTime;
            duration = _totalTime - _elapsedTime;

            if (_elapsedTime >= _totalTime)
            {

                duration = _totalTime;
                _elapsedTime = 0;

                if (_repeatCount != 0 && Count < _repeatCount)
                {
                    OnLoop?.Invoke();
                    Count++;
                }
                else if (_repeatCount >= 0)
                {
                    OnFinish?.Invoke();
                    IsFinished = true;
                    duration = 0f;
                }
                _callback.Invoke();
            }
        }

        public override string ToString()
        {
            return $"{ElapsedTime}s / {duration}";
        }
    }
}
