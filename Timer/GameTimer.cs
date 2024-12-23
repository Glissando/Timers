namespace GameTime
{
    public class GameTimer
    {
        float _elapsedTime = 0f; //The time passed for the current loop
        readonly float _loopInterval = 0f; //the total time that must be passed for a loop
        readonly float _delay; //The amount of delay before the timer can start
        float _delayTime;
        readonly int _loopCount; //How many iterations must this timer go through if it loops
        private float _duration = 0.0f;
        
        public float Duration { get => _loopInterval; }
        /// <summary>
        /// How many iterations must this timer go through if it loops
        /// </summary>
        public int LoopCount { get => _loopCount; }

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
        public float PercentageDone { get => _elapsedTime / _loopInterval; }
        /// <summary>
        /// The total elapsed time that the timer has gone through while running.
        /// </summary>
        public float ElapsedTime { get => _elapsedTime; }
        public float TimeLeft { get => _duration - _elapsedTime; }
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

        public GameTimer(float duration)
        {
            _loopInterval = duration;
        }

        public GameTimer(float duration, Action callback) : this(callback, duration)
        {

        }

        public GameTimer(float loopDuration, int loopCount, Action callback) : this(callback, loopDuration, loopCount)
        {

        }

        public GameTimer(float loopDuration, int loopCount) : this(null, loopDuration, loopCount)
        {

        }

        public GameTimer(float loopDuration, int loopCount, float delay, Action callback) : this(callback, loopDuration, loopCount, delay)
        {

        }

        private GameTimer(Action? callback, float duration, int loopCount = 0, float delay = 0.0f)
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
            _loopInterval = duration;
            _loopCount = loopCount;
            _delay = delay;
            _callback = callback ?? _callback;
        }

        public void Update(float deltaTime)
        {
            if (_delay > 0 && _delayTime < _delay)
            {
                _delayTime += deltaTime;
                _duration = _delayTime + _loopInterval;
                return;
            }

            if (_elapsedTime == 0 && Count == 0)
            {
                OnStart?.Invoke();
            }

            _elapsedTime += deltaTime;
            _duration = _loopInterval - _elapsedTime;

            while (_elapsedTime >= _loopInterval)
            {
                _duration = _loopInterval;
                _elapsedTime = MathF.Max(0f, _elapsedTime - _loopInterval);

                if (_loopCount != 0 && Count < _loopCount)
                {
                    OnLoop?.Invoke();
                    Count++;
                }
                else if (_loopCount >= 0)
                {
                    OnFinish?.Invoke();
                    IsFinished = true;
                    _duration = 0f;
                }
                _callback.Invoke();
            }
        }

        public void Reset()
        {
            _elapsedTime = 0f;
            _delayTime = 0f;
            _duration = 0f;
            Count = 0;
            IsFinished = false;
        }

        public override string ToString()
        {
            return $"{ElapsedTime}s / {_duration}";
        }
    }
}
