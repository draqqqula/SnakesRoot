using ServerEngine.Interfaces.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class Vector2Serializer : IBinarySerializer<Vector2>
{
    public void Serialize(BinaryWriter writer, Vector2 value)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
    }
}
