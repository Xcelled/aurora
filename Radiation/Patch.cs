using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Radiation
{
	internal abstract class Patch
	{
		public string Name { get; }

		protected Patch(string name)
		{
			Name = name;
		}

		public abstract void Apply(ModuleDefinition module);
	}
}
