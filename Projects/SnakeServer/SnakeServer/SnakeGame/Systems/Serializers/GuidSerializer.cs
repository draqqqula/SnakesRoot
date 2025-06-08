using ServerEngine.Interfaces.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class GuidSerializer : IBinarySerializer<Guid>
{
    public void Serialize(BinaryWriter writer, Guid value)
    {
        writer.Write(value.ToByteArray());
    }
}
