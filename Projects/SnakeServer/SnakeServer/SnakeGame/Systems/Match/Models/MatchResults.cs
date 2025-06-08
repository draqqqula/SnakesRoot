using SnakeGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Match.Models;

internal struct MatchResults
{
    public readonly struct TeamResult
    {
        public required TeamColor Team { get; init; }
        public required int Score { get; init; }
    }

    public required TeamResult[] TeamResults { get; init; }
}
