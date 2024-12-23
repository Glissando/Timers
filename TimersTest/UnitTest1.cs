using GameTime;

namespace TimersTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFinish()
        {
            Timers timers = new();
            GameTimer timer = new(5f);
            timers.Add(timer);
            timers.Update(5f);
            Assert.IsTrue(timer.IsFinished);
        }

        [Test]
        public void TestStartCallback()
        {
            Timers timers = new();
            GameTimer timer = new(5f);
            float x = 0;
            timer.OnStart += () => x = 1;
            timers.Add(timer);
            timers.Update(5f);
            Assert.That(x, Is.EqualTo(1));
        }

        [Test]
        public void TestFinishCallback()
        {
            Timers timers = new();
            GameTimer timer = new(5f);
            float x = 0;
            timer.OnFinish += () => x = 1;
            timers.Add(timer);
            timers.Update(5f);
            Assert.That(x, Is.EqualTo(1));
        }

        [Test]
        public void TestLoopCallback()
        {
            Timers timers = new();
            GameTimer timer = new(5f, loopCount: 10);
            int x = 0;
            timer.OnLoop += () => x++;
            timers.Add(timer);

            while (!timer.IsFinished)
            {
                timers.Update(5f);
            }

            Assert.That(x, Is.EqualTo(timer.LoopCount));
        }

        [Test]
        public void TestLoopOverflowCallback()
        {
            Timers timers = new();
            GameTimer timer = new(5f, loopCount: 10);
            int x = 0;
            timer.OnLoop += () => x++;
            timers.Add(timer);

            timer.Update(timer.LoopCount * timer.Duration);

            Assert.That(x, Is.EqualTo(timer.LoopCount));
        }
    }
}