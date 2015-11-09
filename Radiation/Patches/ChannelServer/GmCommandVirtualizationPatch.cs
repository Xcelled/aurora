using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Radiation.Patches.ChannelServer
{
	internal class GmCommandVirtualizationPatch : Patch
	{
		public GmCommandVirtualizationPatch()
			: base("GmCommandManager member virtualization")
		{
			
		}

		public override void Apply(ModuleDefinition module)
		{
			var type = module.GetType("Aura.Channel.Util.GmCommandManager");

			var proc = type.GetMethod("Process");

			proc.IsVirtual = true;
			proc.IsNewSlot = true;
		}
	}
}
