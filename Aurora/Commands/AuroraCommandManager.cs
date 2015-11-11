using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aura.Channel;
using Aura.Channel.Network;
using Aura.Channel.Network.Sending;
using Aura.Channel.Util;
using Aura.Channel.Util.Configuration.Files;
using Aura.Channel.World.Entities;
using Aura.Shared.Util;
using ManyConsole;

namespace Aurora.Commands
{
	internal class AuroraCommandManager : GmCommandManager
	{
		public GmCommandManager OriginalManager { get; }
		public List<ConsoleCommand> Commands { get; }

		private readonly ArgumentTokenizer _tokenizer = new ArgumentTokenizer();
		private readonly CommandsConfFile _conf = ChannelServer.Instance.Conf.Commands;

		public AuroraCommandManager(GmCommandManager original)
		{
			OriginalManager = original;

			Commands = new List<ConsoleCommand>();
			foreach (var cmd in ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(AuroraCommandManager)))
			{
				if (string.IsNullOrEmpty(cmd.Command))
					Log.Error("Command {0} did not call IsCommand!", cmd.GetType().Name);
				else
					Commands.Add(cmd);
			}
		}

		public override bool Process(ChannelClient client, Creature creature, string message)
		{
			if (!Handle(client, creature, message))
				return OriginalManager.Process(client, creature, message);
			return true;
		}

		private bool Handle(ChannelClient client, Creature creature, string message)
		{
			if (message.Length < 2)
				return false;

			var isTargedCommand = message.StartsWith(_conf.Prefix2);
			var isSelfCommand = !isTargedCommand && message.StartsWith(_conf.Prefix.ToString());

			if (!isSelfCommand && !isTargedCommand)
				return false;

			message = message.Substring(isTargedCommand ? _conf.Prefix2.Length : _conf.Prefix.ToString().Length);

			var sender = creature;
			var target = creature;
			var args = _tokenizer.Tokenize(message);

			if (isTargedCommand)
			{
				// Get target player
				if (args.Count < 2 || (target = ChannelServer.Instance.World.GetPlayer(args[1])) == null)
				{
					Send.ServerMessage(creature, Localization.Get("Target not found."));
					return true;
				}

				// Remove target name from the args
				args.RemoveAt(1);
			}
		}
	}
}
