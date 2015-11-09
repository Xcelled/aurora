using Radiation.Patches.Shared;

namespace Radiation.Patches
{
	internal class SharedAssembly : Assembly
	{
		public SharedAssembly()
			: base("Aura.Shared")
		{
			Patches.Add(new WriteHeaderPatch());
		}
	}
}
