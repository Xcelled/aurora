using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radiation.Patches.ChannelServer;

namespace Radiation.Patches
{
	internal class ChannelServerAssembly : Assembly
	{
		public ChannelServerAssembly()
			: base("ChannelServer")
		{
			Patches.Add(new GmCommandVirtualizationPatch());
		}
	}
}
