using ServerEngine.Interfaces.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class BooleanSerializer : IBinarySerializer<bool>
{
    public void Serialize(BinaryWriter writer, bool value)
    {
        writer.Write(value);
    }
}
