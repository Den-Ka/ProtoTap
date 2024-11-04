using System;

namespace Etern0nety.Clicker
{
    public interface IBooster
    {
        float TotalTime { get; }

        event Action Started;
        event Action Finished;
        event Action<int> ScoreAdded;
    }
}