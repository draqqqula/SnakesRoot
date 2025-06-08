using ServerEngine.Interfaces.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class StringSerializer : IBinarySerializer<string>
{
    public void Serialize(BinaryWriter writer, string value)
    {
        writer.Write(value);
    }
}
