using ServerEngine.Interfaces.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Systems.Serializers;

internal class IntArraySerializer : IBinarySerializer<int[]>
{
    public void Serialize(BinaryWriter writer, int[] value)
    {
        writer.Write(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
            writer.Write(value[i]);
        }
    }
}
