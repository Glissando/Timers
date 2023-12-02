namespace Timer
{
    public class Timers
    {
        private List<Timer> _timers = new List<Timer>();
        public static float DeltaTime { get; private set; }

        public const float SECOND = 1.0f;
        public const float HALF_SECOND = 0.5f;
        public const float QUARTER_SECOND = 0.25f;
        public const int INFINITE_REPEAT = -1;

        public Timer this[int index]
        {
            get { return _timers[index]; }
            set { _timers[index] = value; }
        }

        public Timers()
        {
            _timers = new List<Timer>();
        }

        public Timer Add(Action function, float time)
        {
            var timer = new Timer(function, time);
            _timers.Add(timer);
            return timer;
        }

        public Timer Add(Action function, float time, bool repeat, int repeatCount)
        {
            var timer = new Timer(function, time, repeatCount);
            _timers.Add(timer);
            return timer;
        }

        public Timer Add(Timer timer)
        {
            _timers.Add(timer);
            return timer;
        }

        public void Remove(Timer timer)
        {
            _timers.Remove(timer);
        }

        public void Clear()
        {
            _timers.Clear();
        }

        public void Update(float dt)
        {
            for (int i = 0; i < _timers.Count; i++)
            {
                if (!_timers[i].IsPaused)
                {
                    _timers[i].Update(dt);
                    if (_timers[i].IsFinished)
                    {
                        _timers.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}