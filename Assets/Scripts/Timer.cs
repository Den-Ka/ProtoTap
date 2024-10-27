using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etern0nety.Clicker
{
    public class Timer
    {
        public double StartTime { get; private set; }
        public double Duration { get; private set; }
        public double EndTime { get; private set; }

        public bool IsRunning => Time.timeAsDouble < EndTime;
        public float Progress => Duration == 0 ? 1 : (float)Math.Clamp((Time.timeAsDouble - StartTime) / Duration, 0, 1);

        public Timer()
        {
            StartTime = Time.timeAsDouble;
            Duration = 0;
            EndTime = StartTime;
        }

        public Timer(double duration)
        {
            Activate(duration);
        }

        public void Activate(double duration)
        {
            StartTime = Time.timeAsDouble;
            Duration = duration;
            EndTime = StartTime + Duration;
        }
    }
}