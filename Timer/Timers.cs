namespace GameTime
{
    public class Timers
    {
        private List<GameTimer> _timers = new List<GameTimer>();
        public static float DeltaTime { get; private set; }

        public const float SECOND = 1.0f;
        public const float HALF_SECOND = 0.5f;
        public const float QUARTER_SECOND = 0.25f;
        public const int INFINITE_REPEAT = -1;

        public GameTimer this[int index]
        {
            get { return _timers[index]; }
            set { _timers[index] = value; }
        }

        public Timers()
        {
            _timers = new List<GameTimer>();
        }

        /// <summary>
        /// Adds a Timer to this Timers instance
        /// </summary>
        /// <param name="duration">How long this timer will last.</param>
        /// <param name="callback">An action that will be called when this timer finishes.</param>
        /// <returns></returns>
        public GameTimer Add(float duration, Action callback)
        {
            var timer = new GameTimer(duration, callback);
            _timers.Add(timer);
            return timer;
        }

        /// <summary>
        /// Adds a Timer to this Timers instance
        /// </summary>
        /// <param name="duration">How long this timer will last.</param>
        /// <param name="repeatCount">How many times the Timer will repeat</param>
        /// <param name="callback">An action that will be called when this timer repeats.</param>
        /// <returns></returns>
        public GameTimer Add(float duration, int repeatCount, Action callback)
        {
            var timer = new GameTimer(duration, repeatCount, callback);
            _timers.Add(timer);
            return timer;
        }

        public GameTimer Add(GameTimer timer)
        {
            _timers.Add(timer);
            return timer;
        }

        public void Remove(GameTimer timer)
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