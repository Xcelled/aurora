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
	internal class DirCommand : Command
	{
		private readonly CommandNamespace _ns;
		private readonly string _trimPattern;

		public DirCommand(CommandNamespace ns, string trimPattern = ".?", int selfAuth = 0, int targetAuth = 50)
			:base("Dir", "Lists all members of the given namespace", selfAuth, targetAuth)
		{
			_ns = ns;
			_trimPattern = trimPattern;
		}

		public override void Execute(ChannelClient client, Creature sender, Creature target, IList<string> args)
		{
			Send.ServerMessage(target, "Listing of {0}:", args[0].TrimEnd(_trimPattern.ToCharArray()));
			Send.ServerMessage(target, "Namespaces: {0}", string.Join(", ", _ns.Namespaces.Select(n => n.Key)));
			Send.ServerMessage(target, "Commands: {0}", string.Join(", ", _ns.Commands.Select(n => n.Key)));
		}
	}
}
