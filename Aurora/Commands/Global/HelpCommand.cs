using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aura.Channel.Network;
using Aura.Channel.Network.Sending;
using Aura.Channel.World.Entities;

namespace Aurora.Commands.Global
{
	internal class HelpCommand : Command
	{
		public HelpCommand()
			: base("help", "Shows the usage for a command.", 0, 50)
		{
		}

		public override void Execute(ChannelClient client, Creature sender, Creature target, IList<string> args)
		{
			if (args.Count < 2)
			{
				ShowHelp(target);
				return;
			}

			var e = (Parent as CommandNamespace).Resolve(args[1]);

			var cmd = e as Command;

			if (cmd != null && !cmd.HasAuth(target, true))
				e = null;

			if (e == null)
				Send.ServerMessage(sender, "Command or namespace '{0}' not found", args[1]);
			else
				ShowHelp(target, e.GetHelp());
		}
	}
}
