using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etern0nety.Clicker
{
    public class TapAccelerator : MonoBehaviour
    {
        [SerializeField] private float _workingTime = 30f;
        [SerializeField] private float _tapPerSecond = 10;
        [Space]
        [SerializeField] private Player _player;

        public float WorkingTime => _workingTime;
        
        public event Action Started;
        public event Action Finished;
        public event Action<int> ScoreAdded;
        
        public float Progress { get; private set; }
        
        public void Launch()
        {
            StopAllCoroutines();
            StartCoroutine(Acceleration());
        }
        
        private IEnumerator Acceleration()
        {
            Progress = 0f;
            
            Started?.Invoke();
            
            var timer = new Timer(_workingTime);
            float deltaTime = 0f;
            float tapPeriod = 1f / _tapPerSecond;
            int totalTaps = (int)(_tapPerSecond * _workingTime);
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