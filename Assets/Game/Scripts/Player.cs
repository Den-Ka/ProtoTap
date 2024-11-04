using System;
using System.Numerics;
using UnityEngine;

namespace Etern0nety.Clicker
{
    public class Player : MonoBehaviour
    {
        public event Action<BigInteger> ScoreChanged;

        [SerializeField] private BigInteger _score = 0;
        [SerializeField] private int _scoreForTap = 1;

        public BigInteger Score => _score;
        public int ScoreForTap => _scoreForTap;

        public void AddScore(int score)
        {
            _score += score;
            ScoreChanged?.Invoke(_score);
        }

    }
}