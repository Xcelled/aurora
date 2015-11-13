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

namespace Aurora.Commands
{
	internal class AuroraCommandManager : GmCommandManager
	{
		public GmCommandManager OriginalManager { get; }
		public CommandNamespace GlobalNamespace { get; }

		private readonly ArgumentTokenizer _tokenizer = new ArgumentTokenizer();
		private readonly CommandsConfFile _conf = ChannelServer.Instance.Conf.Commands;

		public AuroraCommandManager(GmCommandManager original)
		{
			OriginalManager = original;

			GlobalNamespace = new CommandNamespace("Global", "Root Namespace")
			{

				{ @"^char\.", new CommandNamespace("Character", "Commands relating to characters") }
			};
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

			var isTargetedCommand = message.StartsWith(_conf.Prefix2);
			var isSelfCommand = !isTargetedCommand && message.StartsWith(_conf.Prefix.ToString());

			if (!isSelfCommand && !isTargetedCommand)
				return false;

			message = message.Substring(isTargetedCommand ? _conf.Prefix2.Length : _conf.Prefix.ToString().Length);

			var args = _tokenizer.Tokenize(message);
			if (args.Count < 1)
				return false;

			var cmd = GlobalNamespace.GetCommand(args[0]);
			if (cmd == null)
				return false;

			var sender = creature;
			var target = creature;

			if (isTargetedCommand)
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

			cmd.Execute(client, sender, target, args);

			return true;
		}
	}
}
