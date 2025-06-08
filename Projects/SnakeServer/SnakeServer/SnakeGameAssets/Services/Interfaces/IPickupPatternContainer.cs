using SnakeGameAssets.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameAssets.Services.Interfaces;

public interface IPickupPatternContainer
{
    public IEnumerable<Pattern> Patterns { get; }
}
