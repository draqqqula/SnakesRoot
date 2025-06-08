using ServerEngine.Interfaces.Serialization;
using SnakeGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class TeamColorSerializer : IBinarySerializer<TeamColor>
{
    public void Serialize(BinaryWriter writer, TeamColor value)
    {
        writer.Write((byte)value);
    }
}
