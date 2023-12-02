namespace Timer
{
    public class Timer
    {
        float _elapsedTime = 0; //The time passed for the current loop
        readonly float totalTime = 0; //the total time that must be passed for a loop
        readonly float delay; //The amount of delay before the timer can start
        float _delayTime;
        readonly int _repeatCount; //How many iterations must this timer go through if it loops
        public float duration = 0.0f;
        //How many iterations has it gone through
        public int count { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsPaused { get; set; }
        public float PercentageDone { get => _elapsedTime / totalTime; }
        public float ElapsedTime { get => _elapsedTime; }
        public float TimeLeft { get => duration - _elapsedTime; }
        Action callback = delegate { };

        public Action OnLoop = delegate { };
        public Action OnFinish = delegate { };
        public Action OnStart = delegate { };

        public Timer(Action function, float duration, int repeatCount = 0, float delay = 0.0f)
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
            totalTime = duration;
            this._repeatCount = repeatCount;
            this.delay = delay;
            callback = function;
        }

        public Timer(float time)
        {
            totalTime = time;
        }

        public void Update(float deltaTime)
        {
            if (delay > 0 && _delayTime < delay)
            {
                _delayTime += deltaTime;
                duration = _delayTime + totalTime;
                return;
            }

            if (_elapsedTime == 0 && count == 0)
            {
                OnStart?.Invoke();
            }

            _elapsedTime += deltaTime;
            duration = totalTime - _elapsedTime;

            if (_elapsedTime >= totalTime)
            {

                duration = totalTime;
                _elapsedTime = 0;

                if (_repeatCount != 0 && count < _repeatCount)
                {
                    OnLoop?.Invoke();
                    count++;
                }
                else if (_repeatCount >= 0)
                {
                    OnFinish?.Invoke();
                    IsFinished = true;
                    duration = 0f;
                }
                callback.Invoke();
            }
        }

        public override string ToString()
        {
            return $"{ElapsedTime}s / {duration}";
        }
    }
}
