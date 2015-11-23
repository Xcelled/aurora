using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aura.Channel.Network;
using Aura.Channel.World.Entities;

namespace Aurora.Commands
{
	internal sealed class CommandAlias : Command
	{
		private readonly Command _other;

		public CommandAlias(string name, Command other)
			: base(name, other)
		{
			_other = other;
		}

		public override void Execute(ChannelClient client, Creature sender, Creature target, IList<string> args)
		{
			_other.Execute(client, sender, target, args);
		}

		public override IEnumerable<string> GetHelp()
		{
			return _other.GetHelp();
		}
	}
}
