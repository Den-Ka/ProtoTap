using System;
using System.Collections;
using Etern0nety.DI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etern0nety.Clicker
{
    public class TapBuster : MonoBehaviour, IBooster, IProgression
    {
        [FormerlySerializedAs("_workingTime")] [SerializeField] private float totalTime = 30f;
        [SerializeField] private float _tapPerSecond = 10;

        public float TotalTime => totalTime;
        
        public event Action Started;
        public event Action Finished;
        public event Action<int> ScoreAdded;
        
        public float Progress { get; private set; }
        
        private Player _player;

        public void Initialize(DIContainer container)
        {
            _player = container.Resolve<Player>();
        }
        
        public void Launch()
        {
            StopAllCoroutines();
            StartCoroutine(Acceleration());
        }
        
        private IEnumerator Acceleration()
        {
            Progress = 0f;
            
            Started?.Invoke();
            
            var timer = new Timer(totalTime);
            float deltaTime = 0f;
            float tapPeriod = 1f / _tapPerSecond;
            int totalTaps = (int)(_tapPerSecond * totalTime);
            int tapCounter = 0;

            void AddScore(int scoreToAdd)
            {
                _player.AddScore(scoreToAdd);
                ScoreAdded?.Invoke(scoreToAdd);
                tapCounter++;
            }
            
            while (timer.IsRunning)
            {
                Progress = timer.Progress;
                
                while (deltaTime >= tapPeriod)
                {
                    AddScore(_player.ScoreForTap);
                    deltaTime -= tapPeriod;
                }
                
                deltaTime += Time.deltaTime;

                yield return null;
            }

            while (tapCounter < totalTaps)
            {
                AddScore(_player.ScoreForTap);
            }
            
            Progress = 1f;
            
            Finished?.Invoke();
        }
    }
}