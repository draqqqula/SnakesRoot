using SnakeGame.Systems.Digging.Interfaces;
using SnakeGame.Systems.Modifiers;
using SnakeGame.Systems.Modifiers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Digging;

internal class DiggingSpeedModifier : Modifier<float>
{
    private readonly ISlowdownRateProvider _slowdownRateProvider;
    private readonly float _share;

    public DiggingSpeedModifier(IModifiable<float> inner, ISlowdownRateProvider slowdownRateProvider, float share) : base(inner)
    {
        _slowdownRateProvider = slowdownRateProvider;
        _share = share;
    }

    protected override float Modify(float initial)
    {
        return initial - initial * _share * _slowdownRateProvider.SlowdownRate;
    }
}
