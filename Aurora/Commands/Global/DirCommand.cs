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

		public DirCommand(CommandNamespace ns, int selfAuth = 0, int targetAuth = 50)
			: base("?", "Lists all members of the given namespace", selfAuth, targetAuth, true)
		{
			_ns = ns;
		}

		public override void Execute(ChannelClient client, Creature sender, Creature target, IList<string> args)
		{
			Send.ServerMessage(target, "Listing of '{0}':", _ns.FullyQualifiedName);
			Send.ServerMessage(target, "Namespaces: {0}", string.Join(", ", _ns.Namespaces.Where(n => !n.Hide).Select(n => n.Name)));
			Send.ServerMessage(target, "Commands: {0}", string.Join(", ", _ns.Commands.Where(n => !n.Hide && n.HasAuth(target, false)).Select(n => n.Name)));
		}
	}
}
