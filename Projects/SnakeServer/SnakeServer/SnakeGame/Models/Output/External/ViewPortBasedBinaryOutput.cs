using ServerEngine.Models;

namespace SnakeGame.Models.Output.External;

public class ViewPortBasedBinaryOutput(Dictionary<ClientIdentifier, byte[]> Data)
{
	private readonly Dictionary<ClientIdentifier, byte[]> _data = Data;
	public byte[] this[ClientIdentifier id]
	{
		get => _data.GetValueOrDefault(id, []);
	}
}
