using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aura.Channel.Network;
using Aura.Channel.Util;
using Aura.Channel.World.Entities;
using Aura.Shared.Util;

namespace Aurora.Commands
{
	internal class AuroraCommandManager : GmCommandManager
	{
		public GmCommandManager OriginalManager { get; }

		public AuroraCommandManager(GmCommandManager original)
		{
			OriginalManager = original;
		}

		public override bool Process(ChannelClient client, Creature creature, string message)
		{
			Log.Info("Aurora Commander Invoked");

			return OriginalManager.Process(client, creature, message);
		}
	}
}
