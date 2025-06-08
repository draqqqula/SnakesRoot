using ServerEngine.Models;
using SnakeGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Services.Output.Commands
{
    internal class ChangeScoreCommand : ISerializableCommand
    {
        public TeamScore[] NewScore { get; init; } = [];
        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)2);
            writer.Write((byte)NewScore.Length);
            foreach (var team in NewScore)
            {
                writer.Write((byte)team.Color);
                writer.Write(team.Score);
            }
        }

        public static void To(ClientIdentifier clientId, CommandSender sender, params TeamScore[] values)
        {
            sender.Send(new ChangeScoreCommand() { NewScore = values }, clientId, 2);
        }
    }
}
