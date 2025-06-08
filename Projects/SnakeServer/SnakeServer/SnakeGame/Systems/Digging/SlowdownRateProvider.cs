using SnakeGame.Systems.Digging.Interfaces;
using SnakeGame.Systems.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging
{
    internal class SlowdownRateProvider : ISlowdownRateProvider
    {
        private const float IncreaceRate = 3f;
        private const float DecreaceRate = 16f;
        private const float AbsoluteSlowdown = 7f;
        private const float MaxSlowdown = 9f;

        private float _slowDownValue = 0f;

        public float SlowdownRate => Math.Min(_slowDownValue / AbsoluteSlowdown, 1);

        public void Increace(int amount)
        {
            _slowDownValue = Math.Min(_slowDownValue + amount * IncreaceRate, MaxSlowdown);
        }

        public void Decreace(float deltaTime)
        {
            _slowDownValue = Math.Max(_slowDownValue - deltaTime * DecreaceRate, 0);
        }
    }
}
