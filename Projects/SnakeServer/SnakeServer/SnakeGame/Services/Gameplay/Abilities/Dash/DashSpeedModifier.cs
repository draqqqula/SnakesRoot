using SnakeGame.Systems.Modifiers;
using SnakeGame.Systems.Modifiers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Gameplay.Abilities.Dash;

internal class DashSpeedModifier : Modifier<float>
{
    public DashSpeedModifier(IModifiable<float> inner) : base(inner)
    {
    }

    protected override float Modify(float initial)
    {
        return initial * 3;
    }
}
