using System;
using System.Timers;

namespace DeveloperTimer
{
    public interface ITimeController
    {
        void Listen(Action<object, ElapsedEventArgs> onTick);
        DateTime Now { get; }
        TimeSpan DelaySpan { get; set; }
    }

    public class TimeController : ITimeController
    {
        private readonly Timer timer;

        public TimeController(double interval)
        {
            timer = new Timer(interval);
            DelaySpan = new TimeSpan(0, 15, 0);
        }


        public void Listen(Action<object, ElapsedEventArgs> onTick)
        {
            timer.Elapsed += new ElapsedEventHandler(onTick);
            timer.Start();
        }

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public TimeSpan DelaySpan { get; set; }
    }
}