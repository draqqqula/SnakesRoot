using ServerEngine.Interfaces.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class FloatSerializer : IBinarySerializer<float>
{
    public void Serialize(BinaryWriter writer, float value)
    {
        writer.Write(value);
    }
}
